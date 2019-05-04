from analyzeText import *
from extractText import *
import sys
textList = []
text = extractTextFromPath(sys.argv[1])

textList.append(text)
documents = generateDocuments(textList)
documents = normalizeDocuments(documents)
detectLanguage(documents)
#analyzeSentiment(documents)
extractKeyPhrases(documents)
#extractEntities(documents)