from ibm_watson import AssistantV2
from ibm_watson import ApiException
from pprint import pprint
import json

service = AssistantV2(
    version='2019-05-04',
    iam_apikey='c8P4NEcoKzjnzCywvlGSngdanh5AwtvxwJEl-LAeZs-3',
    #url="https://gateway-lon.watsonplatform.net/assistant/api/v2/assistants/e5cf3b95-bdad-4b9a-b8a5-1e0f565459c6/sessions"
    url='https://gateway-lon.watsonplatform.net/assistant/api'
)
def createSession():
    response = service.create_session(
    assistant_id='e5cf3b95-bdad-4b9a-b8a5-1e0f565459c6').get_result()
    return response['session_id']

def sendMessageToAssistant(sessionId, message):
    response = service.message(
        assistant_id="e5cf3b95-bdad-4b9a-b8a5-1e0f565459c6",
        session_id=sessionId,
        input={
            'message_type': 'text',
            'text': message
        }
    ).get_result()
    return response['output']
def extractFromResult(result):

    responses = result['generic']
    response = ''

    for response in responses:
        if response['response_type'] is "text":
            response = response['text']
    return result['intents'][0]['intent'], response['text'], result['entities']
try:
    sessionId = createSession()
    result = sendMessageToAssistant(sessionId, "Hi, I'd like to see all of my documents that are in german and made between yesterday and 2020")
    intent, response, entities = extractFromResult(result)
    print("Intent: " + intent)
    print("Response: " + response)
    pprint(entities)
    

except ApiException as ex:
    print ("Method failed with status code " + str(ex.code) + ": " + ex.message)
