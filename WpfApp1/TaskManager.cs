using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenAIO
{

    /// <summary>
    /// A simple task manager for spinning up Jobs and/or
    /// stopping them.
    /// </summary>
    public class TaskManager
    {

        private static TaskManager inst;
        private static readonly object theLock = new object();

        private Dictionary<string, Job> jobs;

        private TaskManager()
        {
            jobs = new Dictionary<string, Job>();
        }

        public TaskManager Instance
        {
            get
            {
                lock (theLock)
                {
                    if (inst == null)
                        inst = new TaskManager();

                    return inst;
                }
            }
        }

        /// <summary>
        /// Add a job to the TaskManager.
        /// </summary>
        /// <param name="job"></param>
        public void Add(Job job)
        {
            lock (theLock)
            {
                // Prime the job
                job.Signal(true);

                // Asynchronously start executing the job
                Task.Run(() =>
                {
                    job.Execute();
                });

                // Add job to our mapping.
                jobs.Add(job.ID, job);
            }
        }

        /// <summary>
        /// Removes a Job from the TaskManager effectively stopping the job.
        /// </summary>
        /// <param name="jobID">string ID</param>
        /// <returns>true if removed, else false.</returns>
        public bool Remove(string jobID)
        {
            lock (theLock)
            {
                Job job;

                // Lookup job
                if (jobs.TryGetValue(jobID, out job) && job != null)
                {
                    // Tell the job to stop executing.
                    job.Signal(false);

                    // Remove the job
                    return jobs.Remove(jobID);
                }
            }

            return false;
        }

        public class Job
        {
            // TODO: Fill in what this class should look like!

            private readonly object theLock = new object();
            private bool running;

            public Job(string id)
            {
                this.ID = id;
                this.running = false;
            }

            public string ID
            {
                get;
                internal set;
            }

            /// <summary>
            /// Signals the job to (potentially) change states.
            /// </summary>
            /// <param name="running">bool state.</param>
            internal void Signal(bool running)
            {
                lock (theLock)
                {
                    this.running = running;
                }
            }

            internal void Execute()
            {
                while (true)
                {
                    lock (theLock)
                    {
                        // Check if we are still running.
                        if (!running)
                            break;

                        // Still running, continue processing the request.
                        // Process();
                    }
                }
            }

        }

    }
}
