/* - windows_audio.h ------------------------------------------------------------------------------
* Implementation for the low-level audio for windows
*
* This file is a part of MEOW project.
*
* This software is in public domain, distributed on "AS IS" basis, without technical support,
* and with no warranty, epress or implied, as to its usefulness for any purpose.
*
* Copyright (c) Grim Maple @ 2017
* -------------------------------------------------------------------------------------------------
*/

#include "../audio.h"

void CALLBACK AudioCallback( HWAVEOUT hWave, UINT uMsg, DWORD dwInstance, DWORD b, DWORD c )
{
	audio_device *device = dwInstance;
	switch ( uMsg )
	{
	case WOM_DONE:
		EnterCriticalSection( &( device->crt ) );
		device->block_done = 1;
		LeaveCriticalSection( &( device->crt ) );
	}
}

int GetFreeHeader( audio_device* device )
{
	if ( device->hdrs[ 0 ].dwFlags & WHDR_DONE || device->hdrs[ 0 ].dwFlags == 0)
	{
		if ( device->hdrs[ 0 ].dwFlags & WHDR_PREPARED )
			waveOutUnprepareHeader( device->waveOut, &( device->hdrs[ 0 ] ), sizeof(WAVEHDR) );
		return 0;
	}
	if ( device->hdrs[ 1 ].dwFlags & WHDR_DONE || device->hdrs[ 1 ].dwFlags == 0)
	{
		if( device->hdrs[1].dwFlags & WHDR_PREPARED )
			waveOutUnprepareHeader( device->waveOut, &( device->hdrs[ 0 ] ), sizeof( WAVEHDR ) );
		return 1;
	}

	return -1;
}

audio_device_ptr MEOW_API audio_open( int32 samples, dword bpc, dword channels, dword blockAlign, dword avgBps )
{
	audio_device *device = malloc( sizeof( audio_device ) );
	if ( !device )
		return NULLPTR;
	WAVEFORMATEX wfx;
	wfx.cbSize = 0;
	wfx.nAvgBytesPerSec = avgBps;
	wfx.nBlockAlign = blockAlign;
	wfx.nChannels = channels;
	wfx.nSamplesPerSec = samples;
	wfx.wBitsPerSample = bpc;
	wfx.wFormatTag = WAVE_FORMAT_PCM;
	int t;
	if ( (t = waveOutOpen( &( device->waveOut ), WAVE_MAPPER, &wfx, (DWORD_PTR)AudioCallback, (DWORD_PTR)device, CALLBACK_FUNCTION )) != MMSYSERR_NOERROR )
	{
		
		audio_close( device );
		return 0;
	}
	device->callback = 0;
	memset( &( device->hdrs[ 0 ] ), 0, sizeof( WAVEHDR ) );
	memset( &( device->hdrs[ 1 ] ), 0, sizeof( WAVEHDR ) );
	InitializeCriticalSection( &( device->crt ) );
	device->block_done = 1;

	AudioInsert( device );

	return device;
}

void MEOW_API audio_close( audio_device *device )
{
	waveOutClose( device->waveOut );
	DeleteCriticalSection( &( device->crt ) );
	AudioRemove( device );
	free( device );
}

void MEOW_API audio_write( audio_device* device, void* data, dword size)
{
	EnterCriticalSection( &( device->crt ) );
	int fhdr = GetFreeHeader( device );
	if ( fhdr == -1 )
	{
		device->block_done = 0;
		goto leave;
	}
	if ( device->hdrs[ fhdr ].lpData != 0 )
		free( device->hdrs[ fhdr ].lpData );
	device->hdrs[ fhdr ].lpData = malloc( size );
	memcpy( device->hdrs[ fhdr ].lpData, data, size );
	device->hdrs[ fhdr ].dwBufferLength = size;
	waveOutPrepareHeader( device->waveOut, &( device->hdrs[ fhdr ] ), sizeof( WAVEHDR ) );
	int t = waveOutWrite( device->waveOut, &( device->hdrs[ fhdr ] ), sizeof( WAVEHDR ) );
	fhdr = GetFreeHeader( device );
	if ( fhdr != -1 )
		device->block_done = 1;
leave:
	LeaveCriticalSection( &( device->crt ) );
}

void MEOW_API audio_stop( audio_device* device )
{
	waveOutReset( device->waveOut );
}

void Cycle( audio_device *dev )
{
	// nothing to do pretty much
}