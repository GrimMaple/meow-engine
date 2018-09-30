/* - audio.c --------------------------------------------------------------------------------------
* Implementation for the low-level audio system
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2017
* -------------------------------------------------------------------------------------------------
*/

#include "audio.h"
#include "util/list.h"

list_node *audios = 0;

void AudioInit( )
{
	audios = list_create( );
}

void AudioCycle( )
{
	list_node *t = audios;
	do
	{
		if ( !t->item )
			continue;
		audio_device *device = t->item;
		Cycle(device);
		if ( device->block_done )
		{
			device->block_done = 0;
			if ( device->callback )
				device->callback( );
		}

	} while ( t = t->next );
}

void AudioCleanup( )
{
	list_node volatile *t = audios;
	while ( t->next )
	{
		if ( t->item )
			audio_close( t->item );
		t = t->next;
	}
	list_cleanup( audios );
}

void AudioInsert( audio_device *device )
{
	list_insert( audios, device );
}

void AudioRemove( audio_device *device )
{
	list_remove( audios, device );
}

void MEOW_API audio_subscribe( audio_device *device, audio_callback callback )
{
	device->callback = callback;
}