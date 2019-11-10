using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.Model;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.Common
{
    [TestClass]
    public class UtilitiesTest
    {
        private List<CallNote> _callNotes1;

        private List<CallNote> _callNotes2;

        [TestInitialize]
        public void TestInitialize()
        {
            _callNotes1 = new List<CallNote>
            {
                new CallNote
                {
                    NoteTime = DateTime.ParseExact("02/02/1999 21:14:14", Constants.DateTimeFormat, null),
                    NoteContent = "Note1",
                    ChildCallNotes = new List<ChildCallNote>
                    {
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/1999 23:14:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note12"
                        },
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/1999 23:34:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note123"
                        }
                    }
                },
                new CallNote
                {
                    NoteTime = DateTime.ParseExact("02/02/2000 21:14:14", Constants.DateTimeFormat, null),
                    NoteContent = "Note2",
                    ChildCallNotes = new List<ChildCallNote>
                    {
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/2000 23:14:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note22"
                        },
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/2000 23:34:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note223"
                        }
                    }
                }
            };

            _callNotes2 = new List<CallNote>
            {
                new CallNote
                {
                    NoteTime = DateTime.ParseExact("02/02/1999 21:14:14", Constants.DateTimeFormat, null),
                    NoteContent = "Note1",
                    ChildCallNotes = new List<ChildCallNote>
                    {
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/1999 23:14:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note12"
                        },
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/1999 23:34:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note123"
                        }
                    }
                },
                new CallNote
                {
                    NoteTime = DateTime.ParseExact("02/02/2000 21:14:14", Constants.DateTimeFormat, null),
                    NoteContent = "Note2",
                    ChildCallNotes = new List<ChildCallNote>
                    {
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/2000 23:14:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note22"
                        },
                        new ChildCallNote
                        {
                            NoteTime = DateTime.ParseExact("02/02/2000 23:34:14", Constants.DateTimeFormat, null),
                            NoteContent = "Note223"
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void EqualsAllTest_TwoListWithSameElements_AreEqual()
        {
            Assert.IsTrue(Utilities.EqualsAll(_callNotes1, _callNotes2));
        }

        [TestMethod]
        public void EqualsAllTest_TwoListWithDiffElementNumber_AreNotEqual()
        {
            _callNotes1.Add(new CallNote());
            Assert.IsFalse(Utilities.EqualsAll(_callNotes1, _callNotes2));
        }

        [TestMethod]
        public void EqualsAllTest_TwoEmptyList_AreEqual()
        {
            Assert.IsTrue(Utilities.EqualsAll<CallNote>(null , null));
        }

        [TestMethod]
        public void EqualsAllTest_OneEmptyListOneNotEmpty_AreNotEqual()
        {
            Assert.IsFalse(Utilities.EqualsAll(_callNotes1, null));
        }

        [TestMethod]
        public void EqualsAllTest_TwoListWithSameElementNumberButDiffNoteContent_AreNotEqual()
        {
            _callNotes1[1].NoteContent = null;
            Assert.IsFalse(Utilities.EqualsAll(_callNotes1, _callNotes2));
        }

        [TestMethod]
        public void EqualsAllTest_TwoListWithSameElementNumberButDiffChildNote_AreNotEqual()
        {
            _callNotes1[1].ChildCallNotes = null;
            Assert.IsFalse(Utilities.EqualsAll(_callNotes1, _callNotes2));
        }

        [TestMethod]
        public void EqualsAllTest_TwoListWithSameElementNumberButDiffChildNoteContent_AreNotEqual()
        {
            _callNotes1[1].ChildCallNotes[0].NoteContent = null;
            Assert.IsFalse(Utilities.EqualsAll(_callNotes1, _callNotes2));
        }

        [TestMethod]
        public void GetHashCodeTest_TwoSameItems_HashCodeAreEqual()
        {
            // Two equal objects should have the same hashcode
            Assert.IsTrue(_callNotes1[0].Equals(_callNotes2[0]));
            Assert.AreEqual(_callNotes1[0].GetHashCode(), _callNotes2[0].GetHashCode());

            Assert.IsTrue(_callNotes1[1].Equals(_callNotes2[1]));
            Assert.AreEqual(_callNotes1[1].GetHashCode(), _callNotes2[1].GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeTest_TwoItemsWithDiffHashCode_AreNotEqual()
        {
            // Two objects don't have the same hashcode should not be equal
            _callNotes1[0].ChildCallNotes.Add(new ChildCallNote());
            Assert.AreNotEqual(_callNotes1[0].GetHashCode(), _callNotes2[0].GetHashCode());
            Assert.IsFalse(_callNotes1[0].Equals(_callNotes2[0]));
        }
    }

}
