import http.client, urllib.request, urllib.parse, urllib.error, base64, time, json
from pprint import pprint
def generateJson(id, language, keywords, metadata):
    json = {}
    json['ID'] = id
    json['Language'] = language
    json['Keywords'] = keywords
    json['Metadata'] = metadata
    return json