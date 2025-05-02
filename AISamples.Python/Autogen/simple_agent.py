import asyncio
from autogen import ConversableAgent
import os
from dotenv import load_dotenv

load_dotenv()
async def main():

    openai_api_key = os.environ['OPENAI_API_KEY']
    model = "gpt-3.5-turbo"

    llm_config = {
        "model": model,
        "api_key": openai_api_key,
    }

    agent = ConversableAgent(
        name="chatbot",
        llm_config=llm_config,
        code_execution_config=False,
        human_input_mode="NEVER",
    )

    response = agent.generate_reply(
        messages=[{"role": "user", "content": "Tell me a funny joke."}]
    )
    print(response)

asyncio.run(main()) 