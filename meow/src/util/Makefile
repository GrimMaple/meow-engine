all: image.o list.o tga.o
	ld -r $^ -o util.o
image.o:
	${CC} ${CFLAGS} -c image.c

tga.o:
	${CC} ${CFLAGS} -c tga.c

list.o:
	${CC} ${CFLAGS} -c list.c

clean:
	find -type f -name '*.o' -exec rm {} +