# Insert indexed webpages in between head and tail

import os

def mergeWebPages(dir, head, tail):
	os.chdir(dir)
	
	pages = os.listdir(".")
	
	for page in pages:
		print page
		
		with open(page, 'r') as p:
			content = p.read()
		
		with open(page, 'w+') as p:
			p.write(head + content + tail)
		
	os.chdir("..")

	
def main():

	f = open("head.html", 'r')
	head = f.read()
	
	f = open("tail.html", 'r')
	tail = f.read()
	
	dirs = os.listdir(".")
	
	for dir in dirs:
		if os.path.isdir(dir):
			print dir
			if dir != "Exercise" and dir != "Test" and dir != "Diet":
				mergeWebPages(dir, head, tail)

if __name__ == "__main__":
	main()