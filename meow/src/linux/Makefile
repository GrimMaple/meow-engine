
all: vsync.o time.o linux.o linux_audio.o
	ld -r $^ -o linux_base.o
vsync.o: 
	${CC} ${CFLAGS} -c vsync.c

time.o: 
	${CC} ${CFLAGS} -c time.c

linux.o:
	${CC} ${CFLAGS} -c linux.c

linux_audio.o:
	${CC} ${CFLAGS} -c linux_audio.c

clean: 
	find -type f -name '*.o' -exec rm {} + 