/*	GEAL Test Server: vì
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include "GtRequest.h"

/*	<summary>î{vÌÝè</summary>
	<parameter name="request">v</parameter>
	<parameter name="operation">ì</parameter>
	<parameter name="targetType">ÎÛ^</parameter>
	<parameter name="targetId">ÎÛID</parameter>
*/
extern void GtRequetBaseSet(GT_REQUEST_BASE* request, enum OperationEnum operation, enum TargetTypeEnum targetType, unsigned short targetId) {
	request->bytes = sizeof(*request) - sizeof(char);
	request->operation = operation;
	request->targetType = targetType;
	request->targetId = targetId;
}

/*	<summary>øvÌÝè</summary>
	<parameter name="request">v</parameter>
	<parameter name="operation">ì</parameter>
	<parameter name="targetType">ÎÛ^</parameter>
	<parameter name="targetId">ÎÛID</parameter>
	<parameter name="parameter">ø</parameter>
*/
void GtRequetParameterSet(GT_REQUEST_PARAMTER* request, enum OperationEnum operation, enum TargetTypeEnum targetType, unsigned short targetId, unsigned int parameter) {
	GtRequetBaseSet(&request->base, operation, targetType, targetId);
	request->base.bytes = sizeof(*request) - sizeof(char);
	request->parameter = parameter;
}
