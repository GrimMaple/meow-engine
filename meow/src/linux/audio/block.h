/* - block.h ---------------------------------------------------------------------------------------
 * An audio block for ALSA playback
 *
 * This file is a part of MEOW project.
 *
 * This software is in public domain, distributed on "AS IS" basis, without technical support,
 * and with no warranty, epress or implied, as to its usefulness for any purpose.
 *
 * Copyright (c) Grim Maple @ 2016-2017
 * -------------------------------------------------------------------------------------------------
 */
 #ifndef _BLOCK_H_
 #define _BLOCK_H_

#include "../../api.h"

typedef struct _block
{
	void* data;
	dword position;
	dword length;
	dword flags;
} block;

#endif