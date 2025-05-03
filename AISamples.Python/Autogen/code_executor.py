import asyncio
from autogen import AssistantAgent, UserProxyAgent
import os
from dotenv import load_dotenv

load_dotenv()

openai_api_key = os.environ['OPENAI_API_KEY']
model = "gpt-3.5-turbo"

llm_config = {
    "model": model,
    "api_key": openai_api_key,
}

assistant = AssistantAgent(
name="Assistant",
llm_config=llm_config,
)

user_proxy = UserProxyAgent(
    name="user",
    human_input_mode="ALWAYS",  
    code_execution_config={
        "work_dir": "coding",
        "use_docker": False,  
    },
)

user_proxy.initiate_chat(
    assistant, message="Plot a chart of META and TESLA stock price change."
)