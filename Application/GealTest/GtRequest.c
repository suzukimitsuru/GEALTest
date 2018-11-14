/*	GEAL Test Server: ��M�v����`
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include <stdio.h>
#include "GtRequest.h"

#define _MAX_COUNT	(100)
#define _MAX_BYTES	(32)
static char _table[_MAX_COUNT][_MAX_BYTES];
static int _get = 0;
static int _put = 0;
static int _count = 0;

/*	<summary>������</summary>
	<return>�G���[�R�[�h</return>
*/
void GtRequestInitialize(void) {
	memset(_table, 0, sizeof(_table));
	_get = 0;
	_put = 0;
	_count = 0;
}

/*	<summary>�v���̎擾</summary>
	<return>�v��(NULL:�Ȃ�)</return>
*/
char* GtRequestGet(void) {
	char* result;
	if (_count > 0) {
		result = &_table[_get][0];
		_get = ((_get + 1) < _MAX_COUNT) ? _get + 1 : 0;
	} else {
		result = NULL;
	}
	return result;
}
