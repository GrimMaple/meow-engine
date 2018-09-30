/* - linux_audio.h ---------------------------------------------------------------------------------
 * Linux audio implementation
 *
 * This file is a part of MEOW project.
 *
 * This software is in public domain, distributed on "AS IS" basis, without technical support,
 * and with no warranty, epress or implied, as to its usefulness for any purpose.
 *
 * Copyright (c) Grim Maple @ 2016-2017
 * -------------------------------------------------------------------------------------------------
 */

#ifndef _LINUX_AUDIO_H_
#define _LINUX_AUDIO_H_

#include "audio/alsa_out.h"
#include "audio/block.h"

typedef void ( *audio_callback )( );

typedef struct audio_device
{
	alsa_out            *device;
	block                blocks[2];
	bool                 block_done;
	snd_async_handler_t *handler;
	audio_callback       callback;
} audio_device;

void Cycle( audio_device* );

#endif