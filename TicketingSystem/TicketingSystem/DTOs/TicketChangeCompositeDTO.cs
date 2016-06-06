using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketingSystem.DTOs;

namespace TicketingSystem.DTOs
{
    public class TicketChangeCompositeDTO
    {
        public TaskDto taskDto;
        public ChangeDTO changeDto;

        public TicketChangeCompositeDTO(TaskDto task, ChangeDTO change)
        {
            this.taskDto = task;
            this.changeDto = change;
        }
    }
}