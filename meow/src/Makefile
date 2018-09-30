
all: util/util.o linux/linux_base.o \
	audio.o events.o extensions.o \
	renderer.o window_shared.o
	${CC} ${CFLAGS} -shared -o libmeow.so $^ -lX11 -lGLU -lGL

util/util.o:
	$(MAKE) $(MFLAGS) -e -C util

linux/linux_base.o:
	$(MAKE) $(MFLAGS) -e -C linux

audio.o:
	${CC} ${CFLAGS} -c audio.c

events.o:
	${CC} ${CFLAGS} -c events.c

extensions.o:
	${CC} ${CFLAGS} -c extensions.c

renderer.o:
	${CC} ${CFLAGS} -c renderer.c

window_shared.o:
	${CC} ${CFLAGS} -c window_shared.c