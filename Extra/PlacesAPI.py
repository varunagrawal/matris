# Get Google Places API result

import requests

base_url = "https://maps.googleapis.com/maps/api/place/textsearch/xml?"

dict = {"query" : "hospital", "key" : "AIzaSyAA92VBXjbRxiNFwKNRt29jDj95qQcJKtA", "sensor" : "true", "location" : "17.45,78.38", "radius" : "5000", "types" : "health"}

parameters = "&".join([key+"="+value for key, value in dict.iteritems()])

url = base_url + parameters

r = requests.get(url)

print r.text.encode('utf-8')
