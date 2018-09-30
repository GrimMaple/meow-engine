/* - alsa_out.h ------------------------------------------------------------------------------------
 * Wrapper on low-level pcm_out
 *
 * This file is a part of MEOW project.
 *
 * This software is in public domain, distributed on "AS IS" basis, without technical support,
 * and with no warranty, epress or implied, as to its usefulness for any purpose.
 *
 * Copyright (c) Grim Maple @ 2016-2017
 * -------------------------------------------------------------------------------------------------
 */
 #ifndef _ALSA_OUT_
 #define _ALSA_OUT_
 #define ALSA_PCM_NEW_HW_PARAMS_API
#include <alsa/asoundlib.h>

#include "../../api.h"
#include "block.h"

typedef struct alsa_out
{
	snd_pcm_t *device;
	block     *block;
} alsa_out;

#endif