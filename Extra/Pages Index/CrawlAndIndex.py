## Python Crawler and Indexer for Matris

import urllib2
import HTMLParser
import os

def getWebPage(site):
	print "Getting site: " + site
	r = urllib2.urlopen(site)
	return r.read()

def indexWebPage(site, webpage):
	
	print "Indexing...\n"

	start = webpage.find('<div id="mainContent" class="medium-7 columns" data-oan="mainContent">')
	end = webpage.find('<div id="socialLinksBottom" class="socialLinks regModuleBottom">')
	parser = HTMLParser.HTMLParser()
	
	#if start > -1 and end > -1:
	#	webpage = parser.unescape(webpage[start:end])
	
	webpage = webpage[start:end]
	
	with open(site.split('/')[-1] + ".html", 'w') as f:
		f.write(webpage)

		
def file_readline(f):
	return f.readline().rstrip()
	
	
def main():
	f = open('sites.txt', 'r')
	
	path = 'C:\Users\vaagraw\Desktop\Matris'
	#print sites
	
	n = int(file_readline(f))
	print n
	
	for i in range(n):
		category = file_readline(f)
		n_pages = int(file_readline(f))
		
		
		if category not in os.listdir("."):
			os.mkdir(category)	
		os.chdir(category)
		
		print category
		print n_pages
		print
		
		for i in range(n_pages):
			site = file_readline(f)
			page = getWebPage(site)
			indexWebPage(site, page)
	
		os.chdir("..")
		
	"""for site in sites:
		site = site.rstrip()
		webpage = getWebPage(site)
		indexWebPage(site, webpage)
	"""
	print "done"
	
	raw_input()
	
if __name__ == "__main__":
	main()