import dialogflow
import argparse
import os

project_id = "document-search-chatbot-fc080"
session_id = "test1234"
language_code = "en"

def detect_intent_audio(project_id, session_id, audio_file_path,
                        language_code):

    os.environ["GOOGLE_APPLICATION_CREDENTIALS"]="C:\\Users\\tadija\\Desktop\\project\\Watson\\document-search-chatbot-42134bacfbd1.json"

    import dialogflow_v2 as dialogflow

    session_client = dialogflow.SessionsClient()

    # Note: hard coding audio_encoding and sample_rate_hertz for simplicity.
    audio_encoding = dialogflow.enums.AudioEncoding.AUDIO_ENCODING_LINEAR_16
    sample_rate_hertz = 16000

    session = session_client.session_path(project_id, session_id)
    print('Session path: {}\n'.format(session))

    with open(audio_file_path, 'rb') as audio_file:
        input_audio = audio_file.read()

    audio_config = dialogflow.types.InputAudioConfig(
        audio_encoding=audio_encoding, language_code=language_code,
        sample_rate_hertz=sample_rate_hertz)
    query_input = dialogflow.types.QueryInput(audio_config=audio_config)

    response = session_client.detect_intent(
        session=session, query_input=query_input,
        input_audio=input_audio)

    print('=' * 20)
    print('Query text: {}'.format(response.query_result.query_text))
    print('Detected intent: {} (confidence: {})\n'.format(
        response.query_result.intent.display_name,
        response.query_result.intent_detection_confidence))
    print('Fulfillment text: {}\n'.format(
        response.query_result.fulfillment_text))


detect_intent_audio(project_id, session_id, "C:\\Users\\tadija\\Desktop\\project\\Watson\\SampleAudio_0.4mb.mp3", language_code)