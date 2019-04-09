using GealRsxEnum;
using GEALTest;
using GEALTest.Request;
using System;
using System.Collections.Generic;

namespace GEALTestProgram
{
    public class TestSample
    {
        private Client client;
        public TestSample(Client client)
        {
            this.client = client;
        }
        public void TestRun()
		{
            client.Assert("�X�e�[�W000�̊J�n�҂�",
                client.Wait(new List<RequestBase>() {
                    new UGxStageEnter((ushort)eGE_STAGE_ID.eSTGID_Stage000),
                }, 10 * 1000));

            client.Assert("�X�e�[�W001�ֈڂ�",
                client.Operation(new eGEMSG_BUTTON_CLICK((ushort)eGE_WIDGET_ID.eWGTID_00_NextBtn)));
            client.Assert("�X�e�[�W001�҂�",
                client.Wait(new List<RequestBase>() {
                    new UGxStageEnter((ushort)eGE_STAGE_ID.eSTGID_Stage001),
                }, 1 * 1000));

            client.Assert("�X�e�[�W002�ֈڂ�",
                client.Operation(new eGEMSG_BUTTON_CLICK((ushort)eGE_WIDGET_ID.eWGTID_01_NextBtn)));
            client.Assert("�X�e�[�W003�͑҂��Ă����Ȃ�",
                client.Wait(new List<RequestBase>() {
                new UGxStageEnter((ushort)eGE_STAGE_ID.eSTGID_Stage003),
            }, 1 * 1000));
        }
    }
}
