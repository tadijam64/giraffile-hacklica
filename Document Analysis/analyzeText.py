import requests
# pprint is pretty print (formats the JSON)
from pprint import pprint
from IPython.display import HTML
subscription_key = 'dd8eeb395ae24a15abd2e97b974f0ff9'
assert subscription_key
text_analytics_base_url = "https://westcentralus.api.cognitive.microsoft.com/text/analytics/v2.1/"

# API REFERENCE: https://westus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-V2-1/operations/56f30ceeeda5650db055a3c7


def detectLanguage(documents):
    language_api_url = text_analytics_base_url + "languages"
    print(language_api_url)
    headers   = {"Ocp-Apim-Subscription-Key": subscription_key}
    response  = requests.post(language_api_url, headers=headers, json=documents)
    languages = response.json()
    return languages

def analyzeSentiment(documents):
    sentiment_api_url = text_analytics_base_url + "sentiment"
    print(sentiment_api_url)
    print("Što veći score, pozitivniji je sentiment")
    headers   = {"Ocp-Apim-Subscription-Key": subscription_key}
    response  = requests.post(sentiment_api_url, headers=headers, json=documents)
    sentiments = response.json()
    return sentiments


def extractKeyPhrases(documents):
    key_phrase_api_url = text_analytics_base_url + "keyPhrases"
    print(key_phrase_api_url)
    headers   = {'Ocp-Apim-Subscription-Key': subscription_key}
    response  = requests.post(key_phrase_api_url, headers=headers, json=documents)
    key_phrases = response.json()
    return key_phrases


def extractEntities(documents):
    entity_linking_api_url = text_analytics_base_url + "entities"
    print(entity_linking_api_url)
    headers   = {"Ocp-Apim-Subscription-Key": subscription_key}
    response  = requests.post(entity_linking_api_url, headers=headers, json=documents)
    entities = response.json()
    return entities

def generateDocuments(textList):
    json = {}
    json['documents'] = []
    id = 1
    for text in textList:
        document = {}
        document['id'] = id
        document['text'] = text
        json['documents'].append(document)
        id+=1
    return json

def normalizeDocuments(documents):
    for document in documents['documents']:
        if len(document['text']) > 5000:
            document['text'] = document['text'][:5000]
    return documents