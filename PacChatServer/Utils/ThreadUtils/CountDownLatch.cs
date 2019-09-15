﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacChatServer.Utils.ThreadUtils
{
    class CountdownLatch
    {
        private int m_remain;
        private EventWaitHandle m_event;

        public CountdownLatch(int count)
        {
            m_remain = count;
            m_event = new ManualResetEvent(false);
        }

        public void Signal()
        {
            // The last thread to signal also sets the event.
            if (Interlocked.Decrement(ref m_remain) == 0)
                m_event.Set();
        }

        public void Wait()
        {
            m_event.WaitOne();
        }
    }
}
