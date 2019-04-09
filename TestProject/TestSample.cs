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
            client.Assert("ステージ000の開始待ち",
                client.Wait(new List<RequestBase>() {
                    new UGxStageEnter((ushort)eGE_STAGE_ID.eSTGID_Stage000),
                }, 10 * 1000));

            client.Assert("ステージ001へ移る",
                client.Operation(new eGEMSG_BUTTON_CLICK((ushort)eGE_WIDGET_ID.eWGTID_00_NextBtn)));
            client.Assert("ステージ001待ち",
                client.Wait(new List<RequestBase>() {
                    new UGxStageEnter((ushort)eGE_STAGE_ID.eSTGID_Stage001),
                }, 1 * 1000));

            client.Assert("ステージ002へ移る",
                client.Operation(new eGEMSG_BUTTON_CLICK((ushort)eGE_WIDGET_ID.eWGTID_01_NextBtn)));
            client.Assert("ステージ003は待っても来ない",
                client.Wait(new List<RequestBase>() {
                new UGxStageEnter((ushort)eGE_STAGE_ID.eSTGID_Stage003),
            }, 1 * 1000));
        }
    }
}
