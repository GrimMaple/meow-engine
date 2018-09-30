CC = gcc
CFLAGS= -I "../include" -fPIC -Lasound -Lgl -Lglu
OBJS=

export CC
export CFLAGS
export OBJS

all: meow.so
	mv src/libmeow.so libmeow.so

meow.so:
	$(MAKE) $(MFLAGS) -e -C src

clean:
	find -type f -name '*.*o' -exec rm {} +