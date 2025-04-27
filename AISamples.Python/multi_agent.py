
import asyncio
from semantic_kernel import Kernel
from semantic_kernel.agents import ChatCompletionAgent, ChatHistoryAgentThread
from semantic_kernel.connectors.ai.open_ai import OpenAIChatCompletion
from semantic_kernel.filters import FunctionInvocationContext
import os
from dotenv import load_dotenv

load_dotenv()

openai_api_key = os.environ['OPENAI_API_KEY']

# Define the auto function invocation filter that will be used by the kernel
async def function_invocation_filter(context: FunctionInvocationContext, next):
    """A filter that will be called for each function call in the response."""
    if "messages" not in context.arguments:
        await next(context)
        return
    print(f"    Agent [{context.function.name}] called with messages: {context.arguments['messages']}")
    await next(context)
    print(f"    Response from agent [{context.function.name}]: {context.result.value}")


# Create and configure the kernel.
kernel = Kernel()

# The filter is used for demonstration purposes to show the function invocation.
kernel.add_filter("function_invocation", function_invocation_filter)

billing_agent = ChatCompletionAgent(
    service=OpenAIChatCompletion(ai_model_id="gpt-3.5-turbo", api_key=openai_api_key),
    name="BillingAgent",
    instructions=(
        "You specialize in handling customer questions related to billing issues. "
        "This includes clarifying invoice charges, payment methods, billing cycles, "
        "explaining fees, addressing discrepancies in billed amounts, updating payment details, "
        "assisting with subscription changes, and resolving payment failures. "
        "Your goal is to clearly communicate and resolve issues specifically about payments and charges."
    ),
)

refund_agent = ChatCompletionAgent(
    service=OpenAIChatCompletion(ai_model_id="gpt-3.5-turbo", api_key=openai_api_key),
    name="RefundAgent",
    instructions=(
        "You specialize in addressing customer inquiries regarding refunds. "
        "This includes evaluating eligibility for refunds, explaining refund policies, "
        "processing refund requests, providing status updates on refunds, handling complaints related to refunds, "
        "and guiding customers through the refund claim process. "
        "Your goal is to assist users clearly and empathetically to successfully resolve their refund-related concerns."
    ),
)

triage_agent = ChatCompletionAgent(
    service=OpenAIChatCompletion(ai_model_id="gpt-3.5-turbo", api_key=openai_api_key),
    kernel=kernel,
    name="TriageAgent",
    instructions=(
        "Your role is to evaluate the user's request and forward it to the appropriate agent based on the nature of "
        "the query. Forward requests about charges, billing cycles, payment methods, fees, or payment issues to the "
        "BillingAgent. Forward requests concerning refunds, refund eligibility, refund policies, or the status of "
        "refunds to the RefundAgent. Your goal is accurate identification of the appropriate specialist to ensure the "
        "user receives targeted assistance."
    ),
    plugins=[billing_agent, refund_agent],
)

thread: ChatHistoryAgentThread = None


async def chat() -> bool:
    """
    Continuously prompt the user for input and show the assistant's response.
    Type 'exit' to exit.
    """
    try:
        user_input = input("User:> ")
    except (KeyboardInterrupt, EOFError):
        print("\n\nExiting chat...")
        return False

    if user_input.lower().strip() == "exit":
        print("\n\nExiting chat...")
        return False

    response = await triage_agent.get_response(
        messages=user_input,
        thread=thread,
    )

    if response:
        print(f"Agent :> {response}")

    return True


async def main() -> None:
    print("Welcome to the chat bot!\n  Type 'exit' to exit.\n  Try to get some billing or refund help.")
    chatting = True
    while chatting:
        chatting = await chat()


if __name__ == "__main__":
    asyncio.run(main())