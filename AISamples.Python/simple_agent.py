import asyncio
from semantic_kernel.agents import ChatCompletionAgent
from semantic_kernel.connectors.ai.open_ai import OpenAIChatCompletion
import os
from dotenv import load_dotenv

load_dotenv()
async def main():

    openai_api_key = os.environ['OPENAI_API_KEY']

    # Initialize a chat agent with basic instructions
    agent = ChatCompletionAgent(
        service=OpenAIChatCompletion(ai_model_id="gpt-3.5-turbo", api_key=openai_api_key),
        name="SK-Assistant",
        instructions="You are a helpful assistant.",
    )

    # Get a response to a user message
    response = await agent.get_response(messages="Write 5 sentences about Semantic Kernel.")
    print(response.content)

asyncio.run(main()) 
