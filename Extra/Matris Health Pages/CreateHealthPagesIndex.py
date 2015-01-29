# Create Pages Index file to be consumed by the app

import os

def main():

	f = open("HealthPagesLinks.txt", "w+")
	
	dirs = os.listdir(".")
	
	for dir in dirs:
		if os.path.isdir(dir) and dir != "test":
			
			pages = os.listdir(dir)
			
			for page in pages:
				page_url = page
				page = page[:-5]
				f.write(dir.capitalize() + ";" + " ".join([w.capitalize() for w in page.split('-')]) + ";" + "http://matris.blob.core.windows.net/health/" + dir + "/" + page_url + "\n")
				#f.write(dir.capitalize() + ";" + page + ";" + "http://matris.blob.core.windows.net/matris/" + dir + "/" + page_url + "\n")
	f.close()
	
if __name__ == "__main__":
	main()