from analyzeText import *
from extractText import *
from pprint import pprint
import sys, wikipedia

def generateJson(id, language, keyPhrases, metadata):
    json = {}
    json['ID'] = id
    json['Language'] = language
    json['Keywords'] = keyPhrases
    json['Metadata'] = str(metadata)
    return json

def formatEntities():
    entities = extractEntities(documents)['documents'][0]['entities']
    formattedEntities = []
    for entity in entities:
        json = {}
        json['name'] = entity['name']
        if 'wikipediaUrl' in entity:
            json['wikipediaUrl'] = entity['wikipediaUrl']
            json['wikipediaSummary'] = wikipedia.summary(entity['wikipediaId'], sentences=2)
            json['image'] = wikipedia.page(entity['wikipediaId']).images[0]
        formattedEntities.append(json)
    return formattedEntities

def postToBackend(json):
    requests.post("http://51.144.78.61", data=json)

textList = []
text = extractTextFromPath(sys.argv[1])
textList.append(text)

documents = generateDocuments(textList)
documents = normalizeDocuments(documents)

idFile = sys.argv[2]
language = detectLanguage(documents)['documents'][0]['detectedLanguages'][0]['name']
keyPhrases = extractKeyPhrases(documents)['documents'][0]['keyPhrases'][1:-1]
formattedEntities = formatEntities()
analyzedData = generateJson(idFile, language, keyPhrases, formattedEntities)

postToBackend(analyzedData)


