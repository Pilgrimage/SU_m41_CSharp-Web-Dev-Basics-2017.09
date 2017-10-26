﻿namespace SimpleMvc.App.ViewModels
{
    using System.Collections.Generic;

    public class UserProfileViewModel
    {
        public string Username { get; set; }

        public int OwnerId { get; set; }

        public IEnumerable<NoteViewModel> Notes { get; set; }

    }
}