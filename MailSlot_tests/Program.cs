using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSlot_tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Handle 
        }

        /// <summary>
        /// Create TransMailslotWrite request for client to write data to mailslot on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "mailslotName">The name of maislot to write to. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "writeData">The data to write to mailslot. </param>
        /// <param name = "priority">
        /// This field MUST be in the range of 0 to 9. The larger value being the higher priority. 
        /// </param>
        /// <param name = "className">
        /// The third setup word and the class of the mailslot request. This value MUST be set to one of the following 
        /// values. 
        /// </param>
        /// <returns>a write mailslot request packet </returns>
        private SmbTransMailslotWriteRequestPacket CreateTransMailslotWriteRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            string mailslotName,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            byte[] writeData,
            ushort priority,
            SmbTransMailslotClass className)
        {
            if (mailslotName == null)
            {
                mailslotName = string.Empty;
            }

            SmbTransMailslotWriteRequestPacket packet = new SmbTransMailslotWriteRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = this.capability.MaxParameterCount;
            smbParameters.MaxDataCount = this.capability.MaxDataCount;
            smbParameters.MaxSetupCount = this.capability.MaxSetupCount;
            smbParameters.Flags = transactOptions;
            smbParameters.Timeout = timeOut;
            smbParameters.SetupCount = 3; // the correct count in word of the Setup is always 3.
            smbParameters.Setup = new ushort[3];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_MAILSLOT_WRITE;
            smbParameters.Setup[1] = (ushort)priority;
            smbParameters.Setup[2] = (ushort)className;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(mailslotName, this.capability.IsUnicode);
            smbData.Trans_Data = writeData;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            // update TransactionSubCommand in SmbGlobalContext, record the transact action
            this.capability.TransactionSubCommand = TransSubCommandExtended.TRANS_EXT_MAILSLOT_WRITE;

            return packet;
        }
    }
}
