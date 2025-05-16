import asyncio
from semantic_kernel.agents import ChatCompletionAgent, ChatHistoryAgentThread
from semantic_kernel.connectors.ai.open_ai import OpenAIChatCompletion
from semantic_kernel.connectors.mcp import MCPStdioPlugin
import os
from dotenv import load_dotenv

load_dotenv()

openai_api_key = os.environ['OPENAI_API_KEY']

"""
The following sample demonstrates how to create a chat completion agent that
answers questions about Github using a Semantic Kernel Plugin from a MCP server. 
The Chat Completion Service is passed directly via the ChatCompletionAgent constructor.
Additionally, the plugin is supplied via the constructor.
"""

# Simulate a conversation with the agent
USER_INPUTS = [
    "What are the latest 5 python issues in Microsoft/semantic-kernel?",
    "Are there any untriaged python issues?",
    # "What is the status of issue #10785?",
]

async def main():
    # 1. Create the agent
    async with MCPStdioPlugin(
        name="Github",
        description="Github Plugin",
        command="npx",
        args=["-y", "@modelcontextprotocol/server-github"],
    ) as github_plugin:
        agent = ChatCompletionAgent(
            service=OpenAIChatCompletion(ai_model_id="gpt-4o", api_key=openai_api_key),
            name="IssueAgent",
            instructions="Answer questions about the Microsoft semantic-kernel github project.",
            plugins=[github_plugin],
        )

        for user_input in USER_INPUTS:
            # 2. Create a thread to hold the conversation
            # If no thread is provided, a new thread will be
            # created and returned with the initial response
            thread: ChatHistoryAgentThread | None = None

            print(f"# User: {user_input}")
            # 3. Invoke the agent for a response
            response = await agent.get_response(messages=user_input, thread=thread)
            print(f"# {response.name}: {response} ")
            thread = response.thread

            # 4. Cleanup: Clear the thread
            await thread.delete() if thread else None


if __name__ == "__main__":
    asyncio.run(main())