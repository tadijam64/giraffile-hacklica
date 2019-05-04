#extract-text-from-image.py:
import http.client, urllib.request, urllib.parse, urllib.error, base64, time, json
from pprint import pprint

headers = {
    # Request headers
    'Content-Type': 'application/octet-stream',
    'Ocp-Apim-Subscription-Key': '963b85fc41c94dc7ae5b6bf2b383e769',
}

params = urllib.parse.urlencode({
    # Request parameters
    'mode': 'Printed',
})

def extractTextFromData(data):
    text = ""
    for page in data["recognitionResults"]:
        for line in page["lines"]:
            text += line["text"] + " "
    return text

def requestImageAnalysis(image):
    conn = http.client.HTTPSConnection('westcentralus.api.cognitive.microsoft.com')
    conn.request("POST", "/vision/v2.0/read/core/asyncBatchAnalyze?%s" % params, image, headers)
    response = conn.getresponse()
    opreationLocation = response.getheaders()[4][1].split("/")[-1]
    conn.close()
    return opreationLocation

def getImageAnalysisResult(operationLocation):
    conn = http.client.HTTPSConnection('westcentralus.api.cognitive.microsoft.com')
    conn.request("GET", "/vision/v2.0/read/operations/"+operationLocation+"?%s" % params, "{body}", headers)
    response = conn.getresponse()
    data = response.read()
    conn.close()
    my_json = data.decode('utf8')#.replace("'", '"')
    data = json.loads(r''+my_json)
    return extractTextFromData(data)

def extractTextFrom(image):
    operationLocation = requestImageAnalysis(image)
    time.sleep(10)
    return getImageAnalysisResult(operationLocation)

def extractTextFromPath(docPath):
    doc = open(docPath, 'rb').read()
    operationLocation = requestImageAnalysis(doc)
    if "pdf" or "tiff" in docPath.lower():
        time.sleep(10)
    
    return getImageAnalysisResult(operationLocation)