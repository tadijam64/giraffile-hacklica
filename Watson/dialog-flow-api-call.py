import dialogflow
import argparse
import os

project_id = "document-search-chatbot-fc080"
session_id = "test1234"
texts = "hi boy", "what is up", "how are you"
language_code = "en"

def detect_intent_texts(project_id, session_id, texts, language_code):

    os.environ["GOOGLE_APPLICATION_CREDENTIALS"]="C:\\Users\\tadija\\Desktop\\project\\Watson\\document-search-chatbot-42134bacfbd1.json"

    import dialogflow_v2 as dialogflow
    session_client = dialogflow.SessionsClient()

    session = session_client.session_path(project_id, session_id)
    print('Session path: {}\n'.format(session))

    for text in texts:
        text_input = dialogflow.types.TextInput(
            text=text, language_code=language_code)

        query_input = dialogflow.types.QueryInput(text=text_input)

        response = session_client.detect_intent(
            session=session, query_input=query_input)

        print('=' * 20)
        print('Query text: {}'.format(response.query_result.query_text))
        print('Detected intent: {} (confidence: {})\n'.format(
            response.query_result.intent.display_name,
            response.query_result.intent_detection_confidence))
        print('Fulfillment text: {}\n'.format(
            response.query_result.fulfillment_text))


detect_intent_texts(project_id, session_id, texts, language_code)