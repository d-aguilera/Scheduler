using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.ServiceContracts
{
    public class WcfHelpers
    {
        public static void Using<TChannel>(ChannelFactory<TChannel> factory, Action<TChannel> action)
        {
            TChannel channel = default(TChannel);
            try
            {
                channel = factory.CreateChannel();
                action(channel);
            }
            catch (CommunicationException)
            {
            }
            catch (TimeoutException)
            {
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)channel);
                channel = default(TChannel);
            }
        }

        static void CloseOrAbortServiceChannel(ICommunicationObject communicationObject)
        {
            bool isClosed = false;

            if (communicationObject == null || communicationObject.State == CommunicationState.Closed)
            {
                return;
            }

            try
            {
                if (communicationObject.State != CommunicationState.Faulted)
                {
                    communicationObject.Close();
                    isClosed = true;
                }
            }
            catch (CommunicationException)
            {
            }
            catch (System.TimeoutException)
            {
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // If State was Faulted or any exception occurred while doing the Close(), then do an Abort()
                if (!isClosed)
                {
                    AbortServiceChannel(communicationObject);
                }
            }
        }

        static void AbortServiceChannel(ICommunicationObject communicationObject)
        {
            try
            {
                communicationObject.Abort();
            }
            catch (Exception)
            {
                // An unexpected exception that we don't know how to handle.
                // If we are in this situation:
                // - we should NOT retry the Abort() because it has already failed and there is nothing to suggest it could be successful next time
                // - the abort may have partially succeeded
                // - the actual service call may have been successful
                //
                // The only thing we can do is hope that the channel's resources have been released.
                // Do not rethrow this exception because the actual service operation call might have succeeded
                // and an exception closing the channel should not stop the client doing whatever it does next.
            }
        }
    }
}
