/*	GEAL Test Server: 要求操作
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include "GtRequest.h"

/*	<summary>基本要求の設定</summary>
	<parameter name="request">要求</parameter>
	<parameter name="operation">操作</parameter>
	<parameter name="targetType">対象型</parameter>
	<parameter name="targetId">対象ID</parameter>
*/
extern void GtRequetBaseSet(GT_REQUEST_BASE* request, enum OperationEnum operation, enum TargetTypeEnum targetType, unsigned short targetId) {
	request->bytes = sizeof(*request) - sizeof(char);
	request->operation = operation;
	request->targetType = targetType;
	request->targetId = targetId;
}

/*	<summary>引数要求の設定</summary>
	<parameter name="request">要求</parameter>
	<parameter name="operation">操作</parameter>
	<parameter name="targetType">対象型</parameter>
	<parameter name="targetId">対象ID</parameter>
	<parameter name="parameter">引数</parameter>
*/
void GtRequetParameterSet(GT_REQUEST_PARAMTER* request, enum OperationEnum operation, enum TargetTypeEnum targetType, unsigned short targetId, unsigned int parameter) {
	GtRequetBaseSet(&request->base, operation, targetType, targetId);
	request->base.bytes = sizeof(*request) - sizeof(char);
	request->parameter = parameter;
}
