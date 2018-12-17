/*	GEAL Test Server: �v������
	Copyright (C) 2018 suzukimitsuru, on MIT Licensed.
*/
#include "GtRequest.h"

/*	<summary>��{�v���̐ݒ�</summary>
	<parameter name="request">�v��</parameter>
	<parameter name="operation">����</parameter>
	<parameter name="targetType">�Ώی^</parameter>
	<parameter name="targetId">�Ώ�ID</parameter>
*/
extern void GtRequetBaseSet(GT_REQUEST_BASE* request, enum OperationEnum operation, enum TargetTypeEnum targetType, unsigned short targetId) {
	request->bytes = sizeof(*request) - sizeof(char);
	request->operation = operation;
	request->targetType = targetType;
	request->targetId = targetId;
}

/*	<summary>�����v���̐ݒ�</summary>
	<parameter name="request">�v��</parameter>
	<parameter name="operation">����</parameter>
	<parameter name="targetType">�Ώی^</parameter>
	<parameter name="targetId">�Ώ�ID</parameter>
	<parameter name="parameter">����</parameter>
*/
void GtRequetParameterSet(GT_REQUEST_PARAMTER* request, enum OperationEnum operation, enum TargetTypeEnum targetType, unsigned short targetId, unsigned int parameter) {
	GtRequetBaseSet(&request->base, operation, targetType, targetId);
	request->base.bytes = sizeof(*request) - sizeof(char);
	request->parameter = parameter;
}
