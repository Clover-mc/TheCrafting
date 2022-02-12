﻿// Copyright (c) 2012-2018, Matvei "fragmer" Stefarov <me@matvei.org>
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1. Redistributions of source code must retain the above copyright notice, this
//     list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
// 3. Neither the name of fNbt nor the names of its contributors may be used to
//     endorse or promote products derived from this software without specific
//     prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
// OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Minecraft.Tools.fNbt {
    /// <summary> A tag containing a single byte. </summary>
    public sealed class NbtByte : NbtTag {
        /// <summary> Type of this tag (Byte). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.Byte; }
        }

        /// <summary> Value/payload of this tag (a single byte). </summary>
        public byte Value { get; set; }


        /// <summary> Creates an unnamed NbtByte tag with the default value of 0. </summary>
        public NbtByte() { }


        /// <summary> Creates an unnamed NbtByte tag with the given value. </summary>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtByte(byte value)
            : this(null, value) { }


        /// <summary> Creates an NbtByte tag with the given name and the default value of 0. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtByte(string? tagName)
            : this(tagName, 0) { }


        /// <summary> Creates an NbtByte tag with the given name and value. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtByte(string? tagName, byte value) {
            name = tagName;
            Value = value;
        }


        /// <summary> Creates a copy of given NbtByte tag. </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtByte(NbtByte other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            Value = other.Value;
        }


        internal override bool ReadTag(NbtBinaryReader readStream) {
            if (readStream.Selector != null && !readStream.Selector(this)) {
                readStream.ReadByte();
                return false;
            }

            Value = readStream.ReadByte();
            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            readStream.ReadByte();
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.Byte);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            writeStream.Write(Value);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            writeStream.Write(Value);
        }


        /// <inheritdoc />
        public override object Clone() {
            return new NbtByte(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_Byte");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.Append(": ");
            sb.Append(Value);
        }
    }


    /// <summary> A tag containing an array of bytes. </summary>
    public sealed class NbtByteArray : NbtTag {
        static readonly byte[] ZeroArray = Array.Empty<byte>();

        /// <summary> Type of this tag (ByteArray). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.ByteArray; }
        }

        /// <summary> Value/payload of this tag (an array of bytes). Value is stored as-is and is NOT cloned. May not be <c>null</c>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        public byte[] Value
        {
            get { return bytes; }
            set
            {
                if (value is null) {
                    throw new ArgumentNullException(nameof(value));
                }

                bytes = value;
            }
        }

        byte[] bytes;


        /// <summary> Creates an unnamed NbtByte tag, containing an empty array of bytes. </summary>
        public NbtByteArray()
            : this(null as string) { }


        /// <summary> Creates an unnamed NbtByte tag, containing the given array of bytes. </summary>
        /// <param name="value"> Byte array to assign to this tag's Value. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        /// <remarks> Given byte array will be cloned. To avoid unnecessary copying, call one of the other constructor
        /// overloads (that do not take a byte[]) and then set the Value property yourself. </remarks>
        public NbtByteArray(byte[] value)
            : this(null, value) { }


        /// <summary> Creates an NbtByte tag with the given name, containing an empty array of bytes. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtByteArray(string? tagName) {
            name = tagName;
            bytes = ZeroArray;
        }


        /// <summary> Creates an NbtByte tag with the given name, containing the given array of bytes. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Byte array to assign to this tag's Value. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        /// <remarks> Given byte array will be cloned. To avoid unnecessary copying, call one of the other constructor
        /// overloads (that do not take a byte[]) and then set the Value property yourself. </remarks>
        public NbtByteArray(string? tagName, byte[] value) {
            if (value == null) throw new ArgumentNullException(nameof(value));
            name = tagName;
            bytes = (byte[])value.Clone();
        }


        /// <summary> Creates a deep copy of given NbtByteArray. </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        /// <remarks> Byte array of given tag will be cloned. </remarks>
        public NbtByteArray(NbtByteArray other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            bytes = (byte[])other.Value.Clone();
        }


        /// <summary> Gets or sets a byte at the given index. </summary>
        /// <param name="tagIndex"> The zero-based index of the element to get or set. </param>
        /// <returns> The byte at the specified index. </returns>
        /// <exception cref="IndexOutOfRangeException"> <paramref name="tagIndex"/> is outside the array bounds. </exception>
        public new byte this[int tagIndex]
        {
            get { return Value[tagIndex]; }
            set { Value[tagIndex] = value; }
        }


        internal override bool ReadTag(NbtBinaryReader readStream) {
            int length = readStream.ReadInt32();
            if (length < 0) {
                throw new NbtFormatException("Negative length given in TAG_Byte_Array");
            }

            if (readStream.Selector != null && !readStream.Selector(this)) {
                readStream.Skip(length);
                return false;
            }

            Value = readStream.ReadBytes(length);
            if (Value.Length < length) {
                throw new EndOfStreamException();
            }

            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            int length = readStream.ReadInt32();
            if (length < 0) {
                throw new NbtFormatException("Negative length given in TAG_Byte_Array");
            }

            readStream.Skip(length);
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.ByteArray);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            WriteData(writeStream);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            writeStream.Write(Value.Length);
            writeStream.Write(Value, 0, Value.Length);
        }


        /// <inheritdoc />
        public override object Clone() {
            return new NbtByteArray(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_Byte_Array");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.AppendFormat(": [{0} bytes]", bytes.Length);
        }
    }


    /// <summary> A tag containing a set of other named tags. Order is not guaranteed. </summary>
    public sealed class NbtCompound : NbtTag, ICollection<NbtTag>, ICollection {
        /// <summary> Type of this tag (Compound). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.Compound; }
        }

        readonly Dictionary<string, NbtTag> tags = new Dictionary<string, NbtTag>();


        /// <summary> Creates an empty unnamed NbtByte tag. </summary>
        public NbtCompound() { }


        /// <summary> Creates an empty NbtByte tag with the given name. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtCompound(string? tagName) {
            name = tagName;
        }


        /// <summary> Creates an unnamed NbtByte tag, containing the given tags. </summary>
        /// <param name="tags"> Collection of tags to assign to this tag's Value. May not be null </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tags"/> is <c>null</c>, or one of the tags is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If some of the given tags were not named, or two tags with the same name were given. </exception>
        public NbtCompound(IEnumerable<NbtTag> tags)
            : this(null, tags) { }


        /// <summary> Creates an NbtByte tag with the given name, containing the given tags. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="tags"> Collection of tags to assign to this tag's Value. May not be null </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tags"/> is <c>null</c>, or one of the tags is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If some of the given tags were not named, or two tags with the same name were given. </exception>
        public NbtCompound(string? tagName, IEnumerable<NbtTag> tags) {
            if (tags == null) throw new ArgumentNullException(nameof(tags));
            name = tagName;
            foreach (NbtTag tag in tags) {
                Add(tag);
            }
        }


        /// <summary> Creates a deep copy of given NbtCompound. </summary>
        /// <param name="other"> An existing NbtCompound to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtCompound(NbtCompound other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            foreach (NbtTag tag in other.tags.Values) {
                Add((NbtTag)tag.Clone());
            }
        }


        /// <summary> Gets or sets the tag with the specified name. May return <c>null</c>. </summary>
        /// <returns> The tag with the specified key. Null if tag with the given name was not found. </returns>
        /// <param name="tagName"> The name of the tag to get or set. Must match tag's actual name. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is <c>null</c>; or if trying to assign null value. </exception>
        /// <exception cref="ArgumentException"> <paramref name="tagName"/> does not match the given tag's actual name;
        /// or given tag already has a Parent. </exception>
        public override NbtTag? this[string tagName]
        {
            get { return Get<NbtTag>(tagName); }
            set
            {
                if (tagName == null)
                {
                    throw new ArgumentNullException(nameof(tagName));
                }
                else if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                else if (value.Name != tagName)
                {
                    throw new ArgumentException("Given tag name must match tag's actual name.");
                }
                else if (value.Parent != null)
                {
                    throw new ArgumentException("A tag may only be added to one compound/list at a time.");
                }
                else if (value == this)
                {
                    throw new ArgumentException("Cannot add tag to itself");
                }

                tags[tagName] = value;
                value.Parent = this;
            }
        }


        /// <summary> Gets the tag with the specified name. May return <c>null</c>. </summary>
        /// <param name="tagName"> The name of the tag to get. </param>
        /// <typeparam name="T"> Type to cast the result to. Must derive from NbtTag. </typeparam>
        /// <returns> The tag with the specified key. Null if tag with the given name was not found. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is <c>null</c>. </exception>
        /// <exception cref="InvalidCastException"> If tag could not be cast to the desired tag. </exception>
        public T? Get<T>(string tagName) where T : NbtTag {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));
            if (tags.TryGetValue(tagName, out NbtTag? result))
            {
                return (T)result;
            }

            return null;
        }


        /// <summary> Gets the tag with the specified name. May return <c>null</c>. </summary>
        /// <param name="tagName"> The name of the tag to get. </param>
        /// <returns> The tag with the specified key. Null if tag with the given name was not found. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is <c>null</c>. </exception>
        /// <exception cref="InvalidCastException"> If tag could not be cast to the desired tag. </exception>
        public NbtTag? Get(string tagName) {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));
            if (tags.TryGetValue(tagName, out NbtTag? result))
            {
                return result;
            }

            return null;
        }


        /// <summary> Gets the tag with the specified name. </summary>
        /// <param name="tagName"> The name of the tag to get. </param>
        /// <param name="result"> When this method returns, contains the tag associated with the specified name, if the tag is found;
        /// otherwise, null. This parameter is passed uninitialized. </param>
        /// <typeparam name="T"> Type to cast the result to. Must derive from NbtTag. </typeparam>
        /// <returns> true if the NbtCompound contains a tag with the specified name; otherwise, false. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is <c>null</c>. </exception>
        /// <exception cref="InvalidCastException"> If tag could not be cast to the desired tag. </exception>
        public bool TryGet<T>(string tagName, out T? result) where T : NbtTag {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));
            if (tags.TryGetValue(tagName, out NbtTag? tempResult))
            {
                result = (T)tempResult;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }


        /// <summary> Gets the tag with the specified name. </summary>
        /// <param name="tagName"> The name of the tag to get. </param>
        /// <param name="result"> When this method returns, contains the tag associated with the specified name, if the tag is found;
        /// otherwise, null. This parameter is passed uninitialized. </param>
        /// <returns> true if the NbtCompound contains a tag with the specified name; otherwise, false. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is <c>null</c>. </exception>
        /// <exception cref="InvalidCastException"> If tag could not be cast to the desired tag. </exception>
        public bool TryGet(string tagName, out NbtTag? result) {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));
            if (tags.TryGetValue(tagName, out NbtTag? tempResult))
            {
                result = tempResult;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }


        /// <summary> Adds all tags from the specified collection to this NbtCompound. </summary>
        /// <param name="newTags"> The collection whose elements should be added to this NbtCompound. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="newTags"/> is <c>null</c>, or one of the tags in newTags is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If one of the given tags was unnamed,
        /// or if a tag with the given name already exists in this NbtCompound. </exception>
        public void AddRange(IEnumerable<NbtTag> newTags) {
            if (newTags == null) throw new ArgumentNullException(nameof(newTags));
            foreach (NbtTag tag in newTags) {
                Add(tag);
            }
        }


        /// <summary> Determines whether this NbtCompound contains a tag with a specific name. </summary>
        /// <param name="tagName"> Tag name to search for. May not be <c>null</c>. </param>
        /// <returns> true if a tag with given name was found; otherwise, false. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is <c>null</c>. </exception>
                public bool Contains(string tagName) {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));
            return tags.ContainsKey(tagName);
        }


        /// <summary> Removes the tag with the specified name from this NbtCompound. </summary>
        /// <param name="tagName"> The name of the tag to remove. </param>
        /// <returns> true if the tag is successfully found and removed; otherwise, false.
        /// This method returns false if name is not found in the NbtCompound. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> is <c>null</c>. </exception>
        public bool Remove(string tagName) {
            if (tagName == null) throw new ArgumentNullException(nameof(tagName));
            if (!tags.TryGetValue(tagName, out NbtTag? tag))
            {
                return false;
            }

            tags.Remove(tagName);
            tag.Parent = null;
            return true;
        }


        internal void RenameTag(string oldName, string newName) {
            if (tags.ContainsKey(newName))
            {
                throw new ArgumentException("Cannot rename: a tag with the name already exists in this compound.");
            }

            if (!tags.TryGetValue(oldName, out NbtTag? tag)) {
                throw new ArgumentException("Cannot rename: no tag found to rename.");
            }

            tags.Remove(oldName);
            tags.Add(newName, tag);
        }


        /// <summary> Gets a collection containing all tag names in this NbtCompound. </summary>
        public IEnumerable<string> Names
        {
            get { return tags.Keys; }
        }

        /// <summary> Gets a collection containing all tags in this NbtCompound. </summary>
        public IEnumerable<NbtTag> Tags
        {
            get { return tags.Values; }
        }


        #region Reading / Writing

        internal override bool ReadTag(NbtBinaryReader readStream) {
            if (Parent != null && readStream.Selector != null && !readStream.Selector(this)) {
                SkipTag(readStream);
                return false;
            }

            while (true) {
                NbtTagType nextTag = readStream.ReadTagType();
                NbtTag newTag;
                switch (nextTag) {
                    case NbtTagType.End:
                        return true;

                    case NbtTagType.Byte:
                        newTag = new NbtByte();
                        break;

                    case NbtTagType.Short:
                        newTag = new NbtShort();
                        break;

                    case NbtTagType.Int:
                        newTag = new NbtInt();
                        break;

                    case NbtTagType.Long:
                        newTag = new NbtLong();
                        break;

                    case NbtTagType.Float:
                        newTag = new NbtFloat();
                        break;

                    case NbtTagType.Double:
                        newTag = new NbtDouble();
                        break;

                    case NbtTagType.ByteArray:
                        newTag = new NbtByteArray();
                        break;

                    case NbtTagType.String:
                        newTag = new NbtString();
                        break;

                    case NbtTagType.List:
                        newTag = new NbtList();
                        break;

                    case NbtTagType.Compound:
                        newTag = new NbtCompound();
                        break;

                    case NbtTagType.IntArray:
                        newTag = new NbtIntArray();
                        break;

                    default:
                        throw new NbtFormatException("Unsupported tag type found in NBT_Compound: " + nextTag);
                }

                newTag.Parent = this;
                newTag.Name = readStream.ReadString();
                if (newTag.ReadTag(readStream)) {
                    // ReSharper disable AssignNullToNotNullAttribute
                    // newTag.Name is never null
                    tags.Add(newTag.Name, newTag);
                    // ReSharper restore AssignNullToNotNullAttribute
                }
            }
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            while (true) {
                NbtTagType nextTag = readStream.ReadTagType();
                NbtTag newTag;
                switch (nextTag) {
                    case NbtTagType.End:
                        return;

                    case NbtTagType.Byte:
                        newTag = new NbtByte();
                        break;

                    case NbtTagType.Short:
                        newTag = new NbtShort();
                        break;

                    case NbtTagType.Int:
                        newTag = new NbtInt();
                        break;

                    case NbtTagType.Long:
                        newTag = new NbtLong();
                        break;

                    case NbtTagType.Float:
                        newTag = new NbtFloat();
                        break;

                    case NbtTagType.Double:
                        newTag = new NbtDouble();
                        break;

                    case NbtTagType.ByteArray:
                        newTag = new NbtByteArray();
                        break;

                    case NbtTagType.String:
                        newTag = new NbtString();
                        break;

                    case NbtTagType.List:
                        newTag = new NbtList();
                        break;

                    case NbtTagType.Compound:
                        newTag = new NbtCompound();
                        break;

                    case NbtTagType.IntArray:
                        newTag = new NbtIntArray();
                        break;

                    default:
                        throw new NbtFormatException("Unsupported tag type found in NBT_Compound: " + nextTag);
                }

                readStream.SkipString();
                newTag.SkipTag(readStream);
            }
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.Compound);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            WriteData(writeStream);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            foreach (NbtTag tag in tags.Values) {
                tag.WriteTag(writeStream);
            }

            writeStream.Write(NbtTagType.End);
        }

        #endregion


        #region Implementation of IEnumerable<NbtTag>

        /// <summary> Returns an enumerator that iterates through all tags in this NbtCompound. </summary>
        /// <returns> An IEnumerator&gt;NbtTag&lt; that can be used to iterate through the collection. </returns>
        public IEnumerator<NbtTag> GetEnumerator() {
            return tags.Values.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator() {
            return tags.Values.GetEnumerator();
        }

        #endregion


        #region Implementation of ICollection<NbtTag>

        /// <summary> Adds a tag to this NbtCompound. </summary>
        /// <param name="newTag"> The object to add to this NbtCompound. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="newTag"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If the given tag is unnamed;
        /// or if a tag with the given name already exists in this NbtCompound. </exception>
        public void Add(NbtTag newTag) {
            if (newTag == null) {
                throw new ArgumentNullException(nameof(newTag));
            } else if (newTag == this) {
                throw new ArgumentException("Cannot add tag to self");
            } else if (newTag.Name == null) {
                throw new ArgumentException("Only named tags are allowed in compound tags.");
            } else if (newTag.Parent != null) {
                throw new ArgumentException("A tag may only be added to one compound/list at a time.");
            }

            tags.Add(newTag.Name, newTag);
            newTag.Parent = this;
        }


        /// <summary> Removes all tags from this NbtCompound. </summary>
        public void Clear() {
            foreach (NbtTag tag in tags.Values) {
                tag.Parent = null;
            }

            tags.Clear();
        }


        /// <summary> Determines whether this NbtCompound contains a specific NbtTag.
        /// Looks for exact object matches, not name matches. </summary>
        /// <returns> true if tag is found; otherwise, false. </returns>
        /// <param name="tag"> The object to locate in this NbtCompound. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tag"/> is <c>null</c>. </exception>
                public bool Contains(NbtTag tag) {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            return tags.ContainsValue(tag);
        }


        /// <summary> Copies the tags of the NbtCompound to an array, starting at a particular array index. </summary>
        /// <param name="array"> The one-dimensional array that is the destination of the tag copied from NbtCompound.
        /// The array must have zero-based indexing. </param>
        /// <param name="arrayIndex"> The zero-based index in array at which copying begins. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="array"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> arrayIndex is less than 0. </exception>
        /// <exception cref="ArgumentException"> Given array is multidimensional; arrayIndex is equal to or greater than the length of array;
        /// the number of tags in this NbtCompound is greater than the available space from arrayIndex to the end of the destination array;
        /// or type NbtTag cannot be cast automatically to the type of the destination array. </exception>
        public void CopyTo(NbtTag[] array, int arrayIndex) {
            tags.Values.CopyTo(array, arrayIndex);
        }


        /// <summary> Removes the first occurrence of a specific NbtTag from the NbtCompound.
        /// Looks for exact object matches, not name matches. </summary>
        /// <returns> true if tag was successfully removed from the NbtCompound; otherwise, false.
        /// This method also returns false if tag is not found. </returns>
        /// <param name="tag"> The tag to remove from the NbtCompound. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tag"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If the given tag is unnamed </exception>
        public bool Remove(NbtTag tag) {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (tag.Name == null) throw new ArgumentException("Trying to remove an unnamed tag.");
            if (tags.TryGetValue(tag.Name, out NbtTag? maybeItem))
            {
                if (maybeItem == tag && tags.Remove(tag.Name))
                {
                    tag.Parent = null;
                    return true;
                }
            }

            return false;
        }


        /// <summary> Gets the number of tags contained in the NbtCompound. </summary>
        /// <returns> The number of tags contained in the NbtCompound. </returns>
        public int Count
        {
            get { return tags.Count; }
        }

        bool ICollection<NbtTag>.IsReadOnly
        {
            get { return false; }
        }

        #endregion


        #region Implementation of ICollection

        void ICollection.CopyTo(Array array, int index) {
            CopyTo((NbtTag[])array, index);
        }


        object ICollection.SyncRoot
        {
            get { return (tags as ICollection).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        #endregion


        /// <inheritdoc />
        public override object Clone() {
            return new NbtCompound(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_Compound");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.AppendFormat(": {0} entries {{", tags.Count);

            if (Count > 0) {
                sb.Append('\n');
                foreach (NbtTag tag in tags.Values) {
                    tag.PrettyPrint(sb, indentString, indentLevel + 1);
                    sb.Append('\n');
                }

                for (int i = 0; i < indentLevel; i++) {
                    sb.Append(indentString);
                }
            }

            sb.Append('}');
        }
    }


    /// <summary> A tag containing a double-precision floating point number. </summary>
    public sealed class NbtDouble : NbtTag {
        /// <summary> Type of this tag (Double). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.Double; }
        }

        /// <summary> Value/payload of this tag (a double-precision floating point number). </summary>
        public double Value { get; set; }


        /// <summary> Creates an unnamed NbtDouble tag with the default value of 0. </summary>
        public NbtDouble() { }


        /// <summary> Creates an unnamed NbtDouble tag with the given value. </summary>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtDouble(double value)
            : this(null, value) { }


        /// <summary> Creates an NbtDouble tag with the given name and the default value of 0. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtDouble(string? tagName)
            : this(tagName, 0) { }


        /// <summary> Creates an NbtDouble tag with the given name and value. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtDouble(string? tagName, double value) {
            name = tagName;
            Value = value;
        }


        /// <summary> Creates a copy of given NbtDouble tag. </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtDouble(NbtDouble other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            Value = other.Value;
        }


        internal override bool ReadTag(NbtBinaryReader readStream) {
            if (readStream.Selector != null && !readStream.Selector(this)) {
                readStream.ReadDouble();
                return false;
            }

            Value = readStream.ReadDouble();
            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            readStream.ReadDouble();
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.Double);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            writeStream.Write(Value);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            writeStream.Write(Value);
        }


        /// <inheritdoc />
        public override object Clone() {
            return new NbtDouble(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_Double");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.Append(": ");
            sb.Append(Value);
        }
    }


    /// <summary> A tag containing a single-precision floating point number. </summary>
    public sealed class NbtFloat : NbtTag {
        /// <summary> Type of this tag (Float). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.Float; }
        }

        /// <summary> Value/payload of this tag (a single-precision floating point number). </summary>
        public float Value { get; set; }


        /// <summary> Creates an unnamed NbtFloat tag with the default value of 0f. </summary>
        public NbtFloat() { }


        /// <summary> Creates an unnamed NbtFloat tag with the given value. </summary>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtFloat(float value)
            : this(null, value) { }


        /// <summary> Creates an NbtFloat tag with the given name and the default value of 0f. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtFloat(string? tagName)
            : this(tagName, 0) { }


        /// <summary> Creates an NbtFloat tag with the given name and value. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtFloat(string? tagName, float value) {
            name = tagName;
            Value = value;
        }


        /// <summary> Creates a copy of given NbtFloat tag. </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtFloat(NbtFloat other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            Value = other.Value;
        }


        internal override bool ReadTag(NbtBinaryReader readStream) {
            if (readStream.Selector != null && !readStream.Selector(this)) {
                readStream.ReadSingle();
                return false;
            }

            Value = readStream.ReadSingle();
            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            readStream.ReadSingle();
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.Float);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            writeStream.Write(Value);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            writeStream.Write(Value);
        }


        /// <inheritdoc />
        public override object Clone() {
            return new NbtFloat(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_Float");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.Append(": ");
            sb.Append(Value);
        }
    }


    /// <summary> A tag containing a single signed 32-bit integer. </summary>
    public sealed class NbtInt : NbtTag {
        /// <summary> Type of this tag (Int). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.Int; }
        }

        /// <summary> Value/payload of this tag (a single signed 32-bit integer). </summary>
        public int Value { get; set; }


        /// <summary> Creates an unnamed NbtInt tag with the default value of 0. </summary>
        public NbtInt() { }


        /// <summary> Creates an unnamed NbtInt tag with the given value. </summary>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtInt(int value)
            : this(null, value) { }


        /// <summary> Creates an NbtInt tag with the given name and the default value of 0. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtInt(string? tagName)
            : this(tagName, 0) { }


        /// <summary> Creates an NbtInt tag with the given name and value. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtInt(string? tagName, int value) {
            name = tagName;
            Value = value;
        }


        /// <summary> Creates a copy of given NbtInt tag. </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtInt(NbtInt other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            Value = other.Value;
        }


        internal override bool ReadTag(NbtBinaryReader readStream) {
            if (readStream.Selector != null && !readStream.Selector(this)) {
                readStream.ReadInt32();
                return false;
            }

            Value = readStream.ReadInt32();
            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            readStream.ReadInt32();
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.Int);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            writeStream.Write(Value);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            writeStream.Write(Value);
        }


        /// <inheritdoc />
        public override object Clone() {
            return new NbtInt(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_Int");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.Append(": ");
            sb.Append(Value);
        }
    }


    /// <summary> A tag containing an array of signed 32-bit integers. </summary>
    public sealed class NbtIntArray : NbtTag {
        static readonly int[] ZeroArray = Array.Empty<int>();

        /// <summary> Type of this tag (ByteArray). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.IntArray; }
        }

        /// <summary> Value/payload of this tag (an array of signed 32-bit integers). Value is stored as-is and is NOT cloned. May not be <c>null</c>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        public int[] Value
        {
            get { return ints; }
            set
            {
                if (value is null) {
                    throw new ArgumentNullException(nameof(value));
                }

                ints = value;
            }
        }

        int[] ints;


        /// <summary> Creates an unnamed NbtIntArray tag, containing an empty array of ints. </summary>
        public NbtIntArray()
            : this(null as string) { }


        /// <summary> Creates an unnamed NbtIntArray tag, containing the given array of ints. </summary>
        /// <param name="value"> Int array to assign to this tag's Value. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        /// <remarks> Given int array will be cloned. To avoid unnecessary copying, call one of the other constructor
        /// overloads (that do not take a int[]) and then set the Value property yourself. </remarks>
        public NbtIntArray(int[] value)
            : this(null, value) { }


        /// <summary> Creates an NbtIntArray tag with the given name, containing an empty array of ints. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtIntArray(string? tagName) {
            name = tagName;
            ints = ZeroArray;
        }


        /// <summary> Creates an NbtIntArray tag with the given name, containing the given array of ints. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Int array to assign to this tag's Value. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        /// <remarks> Given int array will be cloned. To avoid unnecessary copying, call one of the other constructor
        /// overloads (that do not take a int[]) and then set the Value property yourself. </remarks>
        public NbtIntArray(string? tagName, int[] value) {
            if (value == null) throw new ArgumentNullException(nameof(value));
            name = tagName;
            ints = (int[])value.Clone();
        }


        /// <summary> Creates a deep copy of given NbtIntArray. </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        /// <remarks> Int array of given tag will be cloned. </remarks>
        public NbtIntArray(NbtIntArray other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            ints = (int[])other.Value.Clone();
        }


        /// <summary> Gets or sets an integer at the given index. </summary>
        /// <param name="tagIndex"> The zero-based index of the element to get or set. </param>
        /// <returns> The integer at the specified index. </returns>
        /// <exception cref="IndexOutOfRangeException"> <paramref name="tagIndex"/> is outside the array bounds. </exception>
        public new int this[int tagIndex]
        {
            get { return Value[tagIndex]; }
            set { Value[tagIndex] = value; }
        }


        internal override bool ReadTag(NbtBinaryReader readStream) {
            int length = readStream.ReadInt32();
            if (length < 0) {
                throw new NbtFormatException("Negative length given in TAG_Int_Array");
            }

            if (readStream.Selector != null && !readStream.Selector(this)) {
                readStream.Skip(length*sizeof(int));
                return false;
            }

            Value = new int[length];
            for (int i = 0; i < length; i++) {
                Value[i] = readStream.ReadInt32();
            }

            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            int length = readStream.ReadInt32();
            if (length < 0) {
                throw new NbtFormatException("Negative length given in TAG_Int_Array");
            }

            readStream.Skip(length*sizeof(int));
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.IntArray);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            WriteData(writeStream);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            writeStream.Write(Value.Length);
            for (int i = 0; i < Value.Length; i++) {
                writeStream.Write(Value[i]);
            }
        }


        /// <inheritdoc />
        public override object Clone() {
            return new NbtIntArray(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_Int_Array");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.AppendFormat(": [{0} ints]", ints.Length);
        }
    }


    /// <summary> A tag containing a list of unnamed tags, all of the same kind. </summary>
    public sealed class NbtList : NbtTag, IList<NbtTag>, IList {
        /// <summary> Type of this tag (List). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.List; }
        }

        readonly List<NbtTag> tags = new List<NbtTag>();

        /// <summary> Gets or sets the tag type of this list. All tags in this NbtTag must be of the same type. </summary>
        /// <exception cref="ArgumentException"> If the given NbtTagType does not match the type of existing list items (for non-empty lists). </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If the given NbtTagType is a recognized tag type. </exception>
        public NbtTagType ListType
        {
            get { return listType; }
            set
            {
                if (value == NbtTagType.End) {
                    // Empty lists may have type "End", see: https://github.com/fragmer/fNbt/issues/12
                    if (tags.Count > 0) {
                        throw new ArgumentException("Only empty list tags may have TagType of End.");
                    }
                } else if (value < NbtTagType.Byte || (value > NbtTagType.IntArray && value != NbtTagType.Unknown)) {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (tags.Count > 0) {
                    NbtTagType actualType = tags[0].TagType;
                    // We can safely assume that ALL tags have the same TagType as the first tag.
                    if (actualType != value) {
                        string msg = String.Format("Given NbtTagType ({0}) does not match actual element type ({1})",
                                                   value, actualType);
                        throw new ArgumentException(msg);
                    }
                }

                listType = value;
            }
        }

        NbtTagType listType;


        /// <summary> Creates an unnamed NbtList with empty contents and undefined ListType. </summary>
        public NbtList()
            : this(null, null, NbtTagType.Unknown) { }


        /// <summary> Creates an NbtList with given name, empty contents, and undefined ListType. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtList(string? tagName)
            : this(tagName, null, NbtTagType.Unknown) { }


        /// <summary> Creates an unnamed NbtList with the given contents, and inferred ListType. 
        /// If given tag array is empty, NbtTagType remains Unknown. </summary>
        /// <param name="tags"> Collection of tags to insert into the list. All tags are expected to be of the same type.
        /// ListType is inferred from the first tag. List may be empty, but may not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tags"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If given tags are of mixed types. </exception>
        public NbtList(IEnumerable<NbtTag> tags)
            : this(null, tags, NbtTagType.Unknown) {
            // the base constructor will allow null "tags," but we don't want that in this constructor
            if (tags == null) throw new ArgumentNullException(nameof(tags));
        }


        /// <summary> Creates an unnamed NbtList with empty contents and an explicitly specified ListType.
        /// If ListType is Unknown, it will be inferred from the type of the first added tag.
        /// Otherwise, all tags added to this list are expected to be of the given type. </summary>
        /// <param name="givenListType"> Name to assign to this tag. May be Unknown. </param>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="givenListType"/> is not a recognized tag type. </exception>
        public NbtList(NbtTagType givenListType)
            : this(null, null, givenListType) { }


        /// <summary> Creates an NbtList with the given name and contents, and inferred ListType. 
        /// If given tag array is empty, NbtTagType remains Unknown. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="tags"> Collection of tags to insert into the list. All tags are expected to be of the same type.
        /// ListType is inferred from the first tag. List may be empty, but may not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tags"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If given tags are of mixed types. </exception>
        public NbtList(string? tagName, IEnumerable<NbtTag> tags)
            : this(tagName, tags, NbtTagType.Unknown) {
            // the base constructor will allow null "tags," but we don't want that in this constructor
            if (tags == null) throw new ArgumentNullException(nameof(tags));
        }


        /// <summary> Creates an unnamed NbtList with the given contents, and an explicitly specified ListType. </summary>
        /// <param name="tags"> Collection of tags to insert into the list.
        /// All tags are expected to be of the same type (matching givenListType).
        /// List may be empty, but may not be <c>null</c>. </param>
        /// <param name="givenListType"> Name to assign to this tag. May be Unknown (to infer type from the first element of tags). </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tags"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="givenListType"/> is not a valid tag type. </exception>
        /// <exception cref="ArgumentException"> If given tags do not match <paramref name="givenListType"/>, or are of mixed types. </exception>
        public NbtList(IEnumerable<NbtTag> tags, NbtTagType givenListType)
            : this(null, tags, givenListType) {
            // the base constructor will allow null "tags," but we don't want that in this constructor
            if (tags == null) throw new ArgumentNullException(nameof(tags));
        }


        /// <summary> Creates an NbtList with the given name, empty contents, and an explicitly specified ListType. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="givenListType"> Name to assign to this tag.
        /// If givenListType is Unknown, ListType will be inferred from the first tag added to this NbtList. </param>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="givenListType"/> is not a valid tag type. </exception>
        public NbtList(string? tagName, NbtTagType givenListType)
            : this(tagName, null, givenListType) { }


        /// <summary> Creates an NbtList with the given name and contents, and an explicitly specified ListType. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="tags"> Collection of tags to insert into the list.
        /// All tags are expected to be of the same type (matching givenListType). May be empty or <c>null</c>. </param>
        /// <param name="givenListType"> Name to assign to this tag. May be Unknown (to infer type from the first element of tags). </param>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="givenListType"/> is not a valid tag type. </exception>
        /// <exception cref="ArgumentException"> If given tags do not match <paramref name="givenListType"/>, or are of mixed types. </exception>
        public NbtList(string? tagName, IEnumerable<NbtTag>? tags, NbtTagType givenListType) {
            name = tagName;
            ListType = givenListType;

            if (tags == null) return;
            foreach (NbtTag tag in tags) {
                Add(tag);
            }
        }


        /// <summary> Creates a deep copy of given NbtList. </summary>
        /// <param name="other"> An existing NbtList to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtList(NbtList other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            listType = other.listType;
            foreach (NbtTag tag in other.tags) {
                tags.Add((NbtTag)tag.Clone());
            }
        }


        /// <summary> Gets or sets the tag at the specified index. </summary>
        /// <returns> The tag at the specified index. </returns>
        /// <param name="tagIndex"> The zero-based index of the tag to get or set. </param>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="tagIndex"/> is not a valid index in the NbtList. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> Given tag's type does not match ListType. </exception>
        public override NbtTag? this[int tagIndex]
        {
            get { return tags[tagIndex]; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                else if (value.Parent != null)
                {
                    throw new ArgumentException("A tag may only be added to one compound/list at a time.");
                }
                else if (value == this || value == Parent)
                {
                    throw new ArgumentException("A list tag may not be added to itself or to its child tag.");
                }
                else if (value.Name != "")
                {
                    throw new ArgumentException("Named tag given. A list may only contain unnamed tags.");
                }

                if (listType != NbtTagType.Unknown && value.TagType != listType)
                {
                    throw new ArgumentException("Items must be of type " + listType);
                }

                tags[tagIndex] = value;
                value.Parent = this;
            }
        }


        /// <summary> Gets or sets the tag with the specified name. </summary>
        /// <param name="tagIndex"> The zero-based index of the tag to get or set. </param>
        /// <typeparam name="T"> Type to cast the result to. Must derive from NbtTag. </typeparam>
        /// <returns> The tag with the specified key. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="tagIndex"/> is not a valid index in the NbtList. </exception>
        /// <exception cref="InvalidCastException"> If tag could not be cast to the desired tag. </exception>
                public T Get<T>(int tagIndex) where T : NbtTag {
            return (T)tags[tagIndex];
        }


        /// <summary> Adds all tags from the specified collection to the end of this NbtList. </summary>
        /// <param name="newTags"> The collection whose elements should be added to this NbtList. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="newTags"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If given tags do not match ListType, or are of mixed types. </exception>
        public void AddRange(IEnumerable<NbtTag> newTags) {
            if (newTags == null) throw new ArgumentNullException(nameof(newTags));
            foreach (NbtTag tag in newTags) {
                Add(tag);
            }
        }


        /// <summary> Copies all tags in this NbtList to an array. </summary>
        /// <returns> Array of NbtTags. </returns>
                // ReSharper disable ReturnTypeCanBeEnumerable.Global
        public NbtTag[] ToArray() {
            // ReSharper restore ReturnTypeCanBeEnumerable.Global
            return tags.ToArray();
        }


        /// <summary> Copies all tags in this NbtList to an array, and casts it to the desired type. </summary>
        /// <typeparam name="T"> Type to cast every member of NbtList to. Must derive from NbtTag. </typeparam>
        /// <returns> Array of NbtTags cast to the desired type. </returns>
        /// <exception cref="InvalidCastException"> If contents of this list cannot be cast to the given type. </exception>
                public T[] ToArray<T>() where T : NbtTag {
            T[]? result = new T[tags.Count];
            for (int i = 0; i < result.Length; i++) {
                result[i] = (T)tags[i];
            }

            return result;
        }


        #region Reading / Writing

        internal override bool ReadTag(NbtBinaryReader readStream) {
            if (readStream.Selector != null && !readStream.Selector(this)) {
                SkipTag(readStream);
                return false;
            }

            ListType = readStream.ReadTagType();

            int length = readStream.ReadInt32();
            if (length < 0) {
                throw new NbtFormatException("Negative list size given.");
            }

            for (int i = 0; i < length; i++) {
                NbtTag newTag = ListType switch
                {
                    NbtTagType.Byte => new NbtByte(),
                    NbtTagType.Short => new NbtShort(),
                    NbtTagType.Int => new NbtInt(),
                    NbtTagType.Long => new NbtLong(),
                    NbtTagType.Float => new NbtFloat(),
                    NbtTagType.Double => new NbtDouble(),
                    NbtTagType.ByteArray => new NbtByteArray(),
                    NbtTagType.String => new NbtString(),
                    NbtTagType.List => new NbtList(),
                    NbtTagType.Compound => new NbtCompound(),
                    NbtTagType.IntArray => new NbtIntArray(),
                    _ => throw new NbtFormatException("Unsupported tag type found in a list: " + ListType),// should never happen, since ListType is checked beforehand
                };
                newTag.Parent = this;
                if (newTag.ReadTag(readStream)) {
                    tags.Add(newTag);
                }
            }

            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            // read list type, and make sure it's defined
            ListType = readStream.ReadTagType();

            int length = readStream.ReadInt32();
            if (length < 0) {
                throw new NbtFormatException("Negative list size given.");
            }

            switch (ListType) {
                case NbtTagType.Byte:
                    readStream.Skip(length);
                    break;
                case NbtTagType.Short:
                    readStream.Skip(length*sizeof(short));
                    break;
                case NbtTagType.Int:
                    readStream.Skip(length*sizeof(int));
                    break;
                case NbtTagType.Long:
                    readStream.Skip(length*sizeof(long));
                    break;
                case NbtTagType.Float:
                    readStream.Skip(length*sizeof(float));
                    break;
                case NbtTagType.Double:
                    readStream.Skip(length*sizeof(double));
                    break;
                default:
                    for (int i = 0; i < length; i++) {
                        switch (listType) {
                            case NbtTagType.ByteArray:
                                new NbtByteArray().SkipTag(readStream);
                                break;
                            case NbtTagType.String:
                                readStream.SkipString();
                                break;
                            case NbtTagType.List:
                                new NbtList().SkipTag(readStream);
                                break;
                            case NbtTagType.Compound:
                                new NbtCompound().SkipTag(readStream);
                                break;
                            case NbtTagType.IntArray:
                                new NbtIntArray().SkipTag(readStream);
                                break;
                        }
                    }

                    break;
            }
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.List);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            WriteData(writeStream);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            if (ListType == NbtTagType.Unknown) {
                throw new NbtFormatException("NbtList had no elements and an Unknown ListType");
            }

            writeStream.Write(ListType);
            writeStream.Write(tags.Count);
            foreach (NbtTag tag in tags) {
                tag.WriteData(writeStream);
            }
        }

        #endregion


        #region Implementation of IEnumerable<NBtTag> and IEnumerable

        /// <summary> Returns an enumerator that iterates through all tags in this NbtList. </summary>
        /// <returns> An IEnumerator&gt;NbtTag&lt; that can be used to iterate through the list. </returns>
        public IEnumerator<NbtTag> GetEnumerator() {
            return tags.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator() {
            return tags.GetEnumerator();
        }

        #endregion


        #region Implementation of IList<NbtTag> and ICollection<NbtTag>

        /// <summary> Determines the index of a specific tag in this NbtList </summary>
        /// <returns> The index of tag if found in the list; otherwise, -1. </returns>
        /// <param name="tag"> The tag to locate in this NbtList. </param>
        public int IndexOf(NbtTag? tag) {
            if (tag == null) return -1;
            return tags.IndexOf(tag);
        }


        /// <summary> Inserts an item to this NbtList at the specified index. </summary>
        /// <param name="tagIndex"> The zero-based index at which newTag should be inserted. </param>
        /// <param name="newTag"> The tag to insert into this NbtList. </param>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="tagIndex"/> is not a valid index in this NbtList. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="newTag"/> is <c>null</c>. </exception>
        public void Insert(int tagIndex, NbtTag newTag) {
            if (newTag == null) {
                throw new ArgumentNullException(nameof(newTag));
            }

            if (listType != NbtTagType.Unknown && newTag.TagType != listType) {
                throw new ArgumentException("Items must be of type " + listType);
            } else if (newTag.Parent != null) {
                throw new ArgumentException("A tag may only be added to one compound/list at a time.");
            }

            tags.Insert(tagIndex, newTag);
            if (listType == NbtTagType.Unknown) {
                listType = newTag.TagType;
            }

            newTag.Parent = this;
        }


        /// <summary> Removes a tag at the specified index from this NbtList. </summary>
        /// <param name="index"> The zero-based index of the item to remove. </param>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index"/> is not a valid index in the NbtList. </exception>
        public void RemoveAt(int index) {
            NbtTag? tag = this[index];
            if (tag is null) return;
            tags.RemoveAt(index);
            tag.Parent = null;
        }


        /// <summary> Adds a tag to this NbtList. </summary>
        /// <param name="newTag"> The tag to add to this NbtList. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="newTag"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If <paramref name="newTag"/> does not match ListType. </exception>
        public void Add(NbtTag newTag) {
            if (newTag == null) {
                throw new ArgumentNullException(nameof(newTag));
            } else if (newTag.Parent != null) {
                throw new ArgumentException("A tag may only be added to one compound/list at a time.");
            } else if (newTag == this || newTag == Parent) {
                throw new ArgumentException("A list tag may not be added to itself or to its child tag.");
            } else if (newTag.Name != "") {
                throw new ArgumentException("Named tag given. A list may only contain unnamed tags.");
            }

            if (listType != NbtTagType.Unknown && newTag.TagType != listType) {
                throw new ArgumentException("Items in this list must be of type " + listType + ". Given type: " +
                                            newTag.TagType);
            }

            tags.Add(newTag);
            newTag.Parent = this;
            if (listType == NbtTagType.Unknown) {
                listType = newTag.TagType;
            }
        }


        /// <summary> Removes all tags from this NbtList. </summary>
        public void Clear() {
            for (int i = 0; i < tags.Count; i++) {
                tags[i].Parent = null;
            }

            tags.Clear();
        }


        /// <summary> Determines whether this NbtList contains a specific tag. </summary>
        /// <returns> true if given tag is found in this NbtList; otherwise, false. </returns>
        /// <param name="item"> The tag to locate in this NbtList. </param>
        public bool Contains(NbtTag item) {
            return tags.Contains(item);
        }


        /// <summary> Copies the tags of this NbtList to an array, starting at a particular array index. </summary>
        /// <param name="array"> The one-dimensional array that is the destination of the tag copied from NbtList.
        /// The array must have zero-based indexing. </param>
        /// <param name="arrayIndex"> The zero-based index in array at which copying begins. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="array"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> arrayIndex is less than 0. </exception>
        /// <exception cref="ArgumentException"> Given array is multidimensional; arrayIndex is equal to or greater than the length of array;
        /// the number of tags in this NbtList is greater than the available space from arrayIndex to the end of the destination array;
        /// or type NbtTag cannot be cast automatically to the type of the destination array. </exception>
        public void CopyTo(NbtTag[] array, int arrayIndex) {
            tags.CopyTo(array, arrayIndex);
        }


        /// <summary> Removes the first occurrence of a specific NbtTag from the NbtCompound.
        /// Looks for exact object matches, not name matches. </summary>
        /// <returns> true if tag was successfully removed from this NbtList; otherwise, false.
        /// This method also returns false if tag is not found. </returns>
        /// <param name="tag"> The tag to remove from this NbtList. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="tag"/> is <c>null</c>. </exception>
        public bool Remove(NbtTag tag) {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (!tags.Remove(tag)) {
                return false;
            }

            tag.Parent = null;
            return true;
        }


        /// <summary> Gets the number of tags contained in the NbtList. </summary>
        /// <returns> The number of tags contained in the NbtList. </returns>
        public int Count
        {
            get { return tags.Count; }
        }

        bool ICollection<NbtTag>.IsReadOnly
        {
            get { return false; }
        }

        #endregion


        #region Implementation of IList and ICollection

        void IList.Remove(object? value)
        {
            if (value is not null) Remove((NbtTag)value);
        }

        object? IList.this[int tagIndex]
        {
            get { return tags[tagIndex]; }
            set { NbtTag? value1 = value as NbtTag; this[tagIndex] = value1; }
        }


        int IList.Add(object? value) {
            if (value is not null)
            {
                Add((NbtTag)value);
                return (tags.Count - 1);
            }
            return tags.Count;
        }


        bool IList.Contains(object? value) {
            return value is not null && tags.Contains((NbtTag)value);
        }


        int IList.IndexOf(object? value) {
            return value is not null ? tags.IndexOf((NbtTag)value) : -1;
        }


        void IList.Insert(int index, object? value) {
            if (value is not null) Insert(index, (NbtTag)value);
        }


        bool IList.IsFixedSize
        {
            get { return false; }
        }


        void ICollection.CopyTo(Array array, int index) {
            CopyTo((NbtTag[])array, index);
        }


        object ICollection.SyncRoot
        {
            get { return (tags as ICollection).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        #endregion


        /// <inheritdoc />
        public override object Clone() {
            return new NbtList(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_List");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.AppendFormat(": {0} entries {{", tags.Count);

            if (Count > 0) {
                sb.Append('\n');
                foreach (NbtTag tag in tags) {
                    tag.PrettyPrint(sb, indentString, indentLevel + 1);
                    sb.Append('\n');
                }

                for (int i = 0; i < indentLevel; i++) {
                    sb.Append(indentString);
                }
            }

            sb.Append('}');
        }
    }


    /// <summary> A tag containing a single signed 64-bit integer. </summary>
    public sealed class NbtLong : NbtTag {
        /// <summary> Type of this tag (Long). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.Long; }
        }

        /// <summary> Value/payload of this tag (a single signed 64-bit integer). </summary>
        public long Value { get; set; }


        /// <summary> Creates an unnamed NbtLong tag with the default value of 0. </summary>
        public NbtLong() { }


        /// <summary> Creates an unnamed NbtLong tag with the given value. </summary>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtLong(long value)
            : this(null, value) { }


        /// <summary> Creates an NbtLong tag with the given name and the default value of 0. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtLong(string tagName)
            : this(tagName, 0) { }


        /// <summary> Creates an NbtLong tag with the given name and value. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtLong(string? tagName, long value) {
            name = tagName;
            Value = value;
        }


        /// <summary> Creates a copy of given NbtLong tag. </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtLong(NbtLong other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            Value = other.Value;
        }


        #region Reading / Writing

        internal override bool ReadTag(NbtBinaryReader readStream) {
            if (readStream.Selector != null && !readStream.Selector(this)) {
                readStream.ReadInt64();
                return false;
            }

            Value = readStream.ReadInt64();
            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            readStream.ReadInt64();
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.Long);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            writeStream.Write(Value);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            writeStream.Write(Value);
        }

        #endregion


        /// <inheritdoc />
        public override object Clone() {
            return new NbtLong(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_Long");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.Append(": ");
            sb.Append(Value);
        }
    }


    /// <summary> A tag containing a single signed 16-bit integer. </summary>
    public sealed class NbtShort : NbtTag {
        /// <summary> Type of this tag (Short). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.Short; }
        }

        /// <summary> Value/payload of this tag (a single signed 16-bit integer). </summary>
        public short Value { get; set; }


        /// <summary> Creates an unnamed NbtShort tag with the default value of 0. </summary>
        public NbtShort() { }


        /// <summary> Creates an unnamed NbtShort tag with the given value. </summary>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtShort(short value)
            : this(null, value) { }


        /// <summary> Creates an NbtShort tag with the given name and the default value of 0. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtShort(string? tagName)
            : this(tagName, 0) { }


        /// <summary> Creates an NbtShort tag with the given name and value. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtShort(string? tagName, short value) {
            name = tagName;
            Value = value;
        }


        /// <summary> Creates a copy of given NbtShort tag. </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtShort(NbtShort other) {
            if (other == null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            Value = other.Value;
        }


        #region Reading / Writing

        internal override bool ReadTag(NbtBinaryReader readStream) {
            if (readStream.Selector != null && !readStream.Selector(this)) {
                readStream.ReadInt16();
                return false;
            }

            Value = readStream.ReadInt16();
            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            readStream.ReadInt16();
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.Short);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            writeStream.Write(Value);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            writeStream.Write(Value);
        }

        #endregion


        /// <inheritdoc />
        public override object Clone() {
            return new NbtShort(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_Short");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.Append(": ");
            sb.Append(Value);
        }
    }


    /// <summary> A tag containing a single string. String is stored in UTF-8 encoding. </summary>
    public sealed class NbtString : NbtTag {
        /// <summary> Type of this tag (String). </summary>
        public override NbtTagType TagType
        {
            get { return NbtTagType.String; }
        }

        /// <summary> Value/payload of this tag (a single string). May not be <c>null</c>. </summary>
        public string Value
        {
            get { return stringVal; }
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));

                stringVal = value;
            }
        }

        string stringVal = "";


        /// <summary> Creates an unnamed NbtString tag with the default value (empty string). </summary>
        public NbtString() { }


        /// <summary> Creates an unnamed NbtString tag with the given value. </summary>
        /// <param name="value"> String value to assign to this tag. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        public NbtString(string value)
            : this(null, value) { }


        /// <summary> Creates an NbtString tag with the given name and value. </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> String value to assign to this tag. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        public NbtString(string? tagName, string value) {
            if (value is null) throw new ArgumentNullException(nameof(value));
            name = tagName;
            Value = value;
        }


        /// <summary> Creates a copy of given NbtString tag. </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtString(NbtString other) {
            if (other is null) throw new ArgumentNullException(nameof(other));
            name = other.name;
            Value = other.Value;
        }


        #region Reading / Writing

        internal override bool ReadTag(NbtBinaryReader readStream) {
            if (readStream.Selector != null && !readStream.Selector(this)) {
                readStream.SkipString();
                return false;
            }

            Value = readStream.ReadString();
            return true;
        }


        internal override void SkipTag(NbtBinaryReader readStream) {
            readStream.SkipString();
        }


        internal override void WriteTag(NbtBinaryWriter writeStream) {
            writeStream.Write(NbtTagType.String);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            writeStream.Write(Value);
        }


        internal override void WriteData(NbtBinaryWriter writeStream) {
            writeStream.Write(Value);
        }

        #endregion


        /// <inheritdoc />
        public override object Clone() {
            return new NbtString(this);
        }


        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentString);
            }

            sb.Append("TAG_String");
            if (!String.IsNullOrEmpty(Name)) {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.Append(": \"");
            sb.Append(Value);
            sb.Append('"');
        }
    }


    /// <summary> Base class for different kinds of named binary tags. </summary>
    public abstract class NbtTag : ICloneable {
        /// <summary> Parent compound tag, either NbtList or NbtCompound, if any.
        /// May be <c>null</c> for detached tags. </summary>
                public NbtTag? Parent { get; internal set; }

        /// <summary> Type of this tag. </summary>
        public abstract NbtTagType TagType { get; }

        /// <summary> Returns true if tags of this type have a value attached.
        /// All tags except Compound, List, and End have values. </summary>
        public bool HasValue
        {
            get
            {
                return TagType switch
                {
                    NbtTagType.Compound or NbtTagType.End or NbtTagType.List or NbtTagType.Unknown => false,
                    _ => true,
                };
            }
        }

        /// <summary> Name of this tag. Immutable, and set by the constructor. May be <c>null</c>. </summary>
        /// <exception cref="ArgumentNullException"> If <paramref name="value"/> is <c>null</c>, and <c>Parent</c> tag is an NbtCompound.
        /// Name of tags inside an <c>NbtCompound</c> may not be null. </exception>
        /// <exception cref="ArgumentException"> If this tag resides in an <c>NbtCompound</c>, and a sibling tag with the name already exists. </exception>
        public string Name
        {
            get { return name is null ? "" : name; }
            set
            {
                if (name == value) {
                    return;
                }

                NbtCompound? parentAsCompound = Parent as NbtCompound;
                if (parentAsCompound is not null) {
                    if (value == null) {
                        throw new ArgumentNullException(nameof(value),
                                                        "Name of tags inside an NbtCompound may not be null.");
                    } else if (name != null) {
                        parentAsCompound.RenameTag(name, value);
                    }
                }

                name = value;
            }
        }

        // Used by impls to bypass setter checks (and avoid side effects) when initializing state
        internal string? name;

        /// <summary> Gets the full name of this tag, including all parent tag names, separated by dots. 
        /// Unnamed tags show up as empty strings. </summary>
        public string Path
        {
            get
            {
                if (Parent is null) {
                    return Name ?? "";
                }

                if (Parent is NbtList parentAsList)
                {
                    return parentAsList.Path + '[' + parentAsList.IndexOf(this) + ']';
                }
                else
                {
                    return Parent.Path + '.' + Name;
                }
            }
        }

        internal abstract bool ReadTag(NbtBinaryReader readStream);

        internal abstract void SkipTag(NbtBinaryReader readStream);

        internal abstract void WriteTag(NbtBinaryWriter writeReader);


        // WriteData does not write the tag's ID byte or the name
        internal abstract void WriteData(NbtBinaryWriter writeReader);


        #region Shortcuts

        /// <summary> Gets or sets the tag with the specified name. May return <c>null</c>. </summary>
        /// <returns> The tag with the specified key. Null if tag with the given name was not found. </returns>
        /// <param name="tagName"> The name of the tag to get or set. Must match tag's actual name. </param>
        /// <exception cref="InvalidOperationException"> If used on a tag that is not NbtCompound. </exception>
        /// <remarks> ONLY APPLICABLE TO NbtCompound OBJECTS!
        /// Included in NbtTag base class for programmers' convenience, to avoid extra type casts. </remarks>
        public virtual NbtTag? this[string tagName]
        {
            get { throw new InvalidOperationException("String indexers only work on NbtCompound tags."); }
            set { throw new InvalidOperationException("String indexers only work on NbtCompound tags."); }
        }

        /// <summary> Gets or sets the tag at the specified index. </summary>
        /// <returns> The tag at the specified index. </returns>
        /// <param name="tagIndex"> The zero-based index of the tag to get or set. </param>
        /// <exception cref="ArgumentOutOfRangeException"> tagIndex is not a valid index in this tag. </exception>
        /// <exception cref="ArgumentNullException"> Given tag is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> Given tag's type does not match ListType. </exception>
        /// <exception cref="InvalidOperationException"> If used on a tag that is not NbtList, NbtByteArray, or NbtIntArray. </exception>
        /// <remarks> ONLY APPLICABLE TO NbtList, NbtByteArray, and NbtIntArray OBJECTS!
        /// Included in NbtTag base class for programmers' convenience, to avoid extra type casts. </remarks>
        public virtual NbtTag? this[int tagIndex]
        {
            get { throw new InvalidOperationException("Integer indexers only work on NbtList tags."); }
            set { throw new InvalidOperationException("Integer indexers only work on NbtList tags."); }
        }

        /// <summary> Returns the value of this tag, cast as a byte.
        /// Only supported by NbtByte tags. </summary>
        /// <exception cref="InvalidCastException"> When used on a tag other than NbtByte. </exception>
        public byte ByteValue
        {
            get
            {
                if (TagType == NbtTagType.Byte) {
                    return ((NbtByte)this).Value;
                } else {
                    throw new InvalidCastException("Cannot get ByteValue from " + GetCanonicalTagName(TagType));
                }
            }
        }

        /// <summary> Returns the value of this tag, cast as a short (16-bit signed integer).
        /// Only supported by NbtByte and NbtShort. </summary>
        /// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
        public short ShortValue
        {
            get
            {
                return TagType switch
                {
                    NbtTagType.Byte => ((NbtByte)this).Value,
                    NbtTagType.Short => ((NbtShort)this).Value,
                    _ => throw new InvalidCastException("Cannot get ShortValue from " + GetCanonicalTagName(TagType)),
                };
            }
        }

        /// <summary> Returns the value of this tag, cast as an int (32-bit signed integer).
        /// Only supported by NbtByte, NbtShort, and NbtInt. </summary>
        /// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
        public int IntValue
        {
            get
            {
                return TagType switch
                {
                    NbtTagType.Byte => ((NbtByte)this).Value,
                    NbtTagType.Short => ((NbtShort)this).Value,
                    NbtTagType.Int => ((NbtInt)this).Value,
                    _ => throw new InvalidCastException("Cannot get IntValue from " + GetCanonicalTagName(TagType)),
                };
            }
        }

        /// <summary> Returns the value of this tag, cast as a long (64-bit signed integer).
        /// Only supported by NbtByte, NbtShort, NbtInt, and NbtLong. </summary>
        /// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
        public long LongValue
        {
            get
            {
                return TagType switch
                {
                    NbtTagType.Byte => ((NbtByte)this).Value,
                    NbtTagType.Short => ((NbtShort)this).Value,
                    NbtTagType.Int => ((NbtInt)this).Value,
                    NbtTagType.Long => ((NbtLong)this).Value,
                    _ => throw new InvalidCastException("Cannot get LongValue from " + GetCanonicalTagName(TagType)),
                };
            }
        }

        /// <summary> Returns the value of this tag, cast as a long (64-bit signed integer).
        /// Only supported by NbtFloat and, with loss of precision, by NbtDouble, NbtByte, NbtShort, NbtInt, and NbtLong. </summary>
        /// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
        public float FloatValue
        {
            get
            {
                return TagType switch
                {
                    NbtTagType.Byte => ((NbtByte)this).Value,
                    NbtTagType.Short => ((NbtShort)this).Value,
                    NbtTagType.Int => ((NbtInt)this).Value,
                    NbtTagType.Long => ((NbtLong)this).Value,
                    NbtTagType.Float => ((NbtFloat)this).Value,
                    NbtTagType.Double => (float)((NbtDouble)this).Value,
                    _ => throw new InvalidCastException("Cannot get FloatValue from " + GetCanonicalTagName(TagType)),
                };
            }
        }

        /// <summary> Returns the value of this tag, cast as a long (64-bit signed integer).
        /// Only supported by NbtFloat, NbtDouble, and, with loss of precision, by NbtByte, NbtShort, NbtInt, and NbtLong. </summary>
        /// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
        public double DoubleValue
        {
            get
            {
                return TagType switch
                {
                    NbtTagType.Byte => ((NbtByte)this).Value,
                    NbtTagType.Short => ((NbtShort)this).Value,
                    NbtTagType.Int => ((NbtInt)this).Value,
                    NbtTagType.Long => ((NbtLong)this).Value,
                    NbtTagType.Float => ((NbtFloat)this).Value,
                    NbtTagType.Double => ((NbtDouble)this).Value,
                    _ => throw new InvalidCastException("Cannot get DoubleValue from " + GetCanonicalTagName(TagType)),
                };
            }
        }

        /// <summary> Returns the value of this tag, cast as a byte array.
        /// Only supported by NbtByteArray tags. </summary>
        /// <exception cref="InvalidCastException"> When used on a tag other than NbtByteArray. </exception>
        public byte[] ByteArrayValue
        {
            get
            {
                if (TagType == NbtTagType.ByteArray) {
                    return ((NbtByteArray)this).Value;
                } else {
                    throw new InvalidCastException("Cannot get ByteArrayValue from " + GetCanonicalTagName(TagType));
                }
            }
        }

        /// <summary> Returns the value of this tag, cast as an int array.
        /// Only supported by NbtIntArray tags. </summary>
        /// <exception cref="InvalidCastException"> When used on a tag other than NbtIntArray. </exception>
        public int[] IntArrayValue
        {
            get
            {
                if (TagType == NbtTagType.IntArray) {
                    return ((NbtIntArray)this).Value;
                } else {
                    throw new InvalidCastException("Cannot get IntArrayValue from " + GetCanonicalTagName(TagType));
                }
            }
        }

        /// <summary> Returns the value of this tag, cast as a string.
        /// Returns exact value for NbtString, and stringified (using InvariantCulture) value for NbtByte, NbtDouble, NbtFloat, NbtInt, NbtLong, and NbtShort.
        /// Not supported by NbtCompound, NbtList, NbtByteArray, or NbtIntArray. </summary>
        /// <exception cref="InvalidCastException"> When used on an unsupported tag. </exception>
        public string StringValue
        {
            get
            {
                return TagType switch
                {
                    NbtTagType.String => ((NbtString)this).Value,
                    NbtTagType.Byte => ((NbtByte)this).Value.ToString(CultureInfo.InvariantCulture),
                    NbtTagType.Double => ((NbtDouble)this).Value.ToString(CultureInfo.InvariantCulture),
                    NbtTagType.Float => ((NbtFloat)this).Value.ToString(CultureInfo.InvariantCulture),
                    NbtTagType.Int => ((NbtInt)this).Value.ToString(CultureInfo.InvariantCulture),
                    NbtTagType.Long => ((NbtLong)this).Value.ToString(CultureInfo.InvariantCulture),
                    NbtTagType.Short => ((NbtShort)this).Value.ToString(CultureInfo.InvariantCulture),
                    _ => throw new InvalidCastException("Cannot get StringValue from " + GetCanonicalTagName(TagType)),
                };
            }
        }

        #endregion


        /// <summary> Returns a canonical (Notchy) name for the given NbtTagType,
        /// e.g. "TAG_Byte_Array" for NbtTagType.ByteArray </summary>
        /// <param name="type"> NbtTagType to name. </param>
        /// <returns> String representing the canonical name of a tag,
        /// or null of given TagType does not have a canonical name (e.g. Unknown). </returns>
        public static string? GetCanonicalTagName(NbtTagType type) {
            return type switch
            {
                NbtTagType.Byte => "TAG_Byte",
                NbtTagType.ByteArray => "TAG_Byte_Array",
                NbtTagType.Compound => "TAG_Compound",
                NbtTagType.Double => "TAG_Double",
                NbtTagType.End => "TAG_End",
                NbtTagType.Float => "TAG_Float",
                NbtTagType.Int => "TAG_Int",
                NbtTagType.IntArray => "TAG_Int_Array",
                NbtTagType.List => "TAG_List",
                NbtTagType.Long => "TAG_Long",
                NbtTagType.Short => "TAG_Short",
                NbtTagType.String => "TAG_String",
                _ => null,
            };
        }


        /// <summary> Prints contents of this tag, and any child tags, to a string.
        /// Indents the string using multiples of the given indentation string. </summary>
        /// <returns> A string representing contents of this tag, and all child tags (if any). </returns>
        public override string ToString() {
            return ToString(DefaultIndentString);
        }


        /// <summary> Creates a deep copy of this tag. </summary>
        /// <returns> A new NbtTag object that is a deep copy of this instance. </returns>
        public abstract object Clone();


        /// <summary> Prints contents of this tag, and any child tags, to a string.
        /// Indents the string using multiples of the given indentation string. </summary>
        /// <param name="indentString"> String to be used for indentation. </param>
        /// <returns> A string representing contents of this tag, and all child tags (if any). </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="indentString"/> is <c>null</c>. </exception>
        public string ToString(string indentString) {
            if (indentString == null) throw new ArgumentNullException(nameof(indentString));
            StringBuilder? sb = new StringBuilder();
            PrettyPrint(sb, indentString, 0);
            return sb.ToString();
        }


        internal abstract void PrettyPrint(StringBuilder sb, string indentString, int indentLevel);

        /// <summary> String to use for indentation in NbtTag's and NbtFile's ToString() methods by default. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        public static string DefaultIndentString
        {
            get { return defaultIndentString; }
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));
                defaultIndentString = value;
            }
        }

        static string defaultIndentString = "  ";
    }


    // Class used to count bytes read-from/written-to non-seekable streams.
    internal class ByteCountingStream : Stream {
        readonly Stream baseStream;

        // These are necessary to avoid counting bytes twice if ReadByte/WriteByte call Read/Write internally.
        bool readingOneByte, writingOneByte;

        // These are necessary to avoid counting bytes twice if Read/Write call ReadByte/WriteByte internally.
        bool readingManyBytes, writingManyBytes;


        public ByteCountingStream(Stream stream) {
            baseStream = stream;
        }


        public override void Flush() {
            baseStream.Flush();
        }


        public override long Seek(long offset, SeekOrigin origin) {
            return baseStream.Seek(offset, origin);
        }


        public override void SetLength(long value) {
            baseStream.SetLength(value);
        }


        public override int Read(byte[] buffer, int offset, int count) {
            readingManyBytes = true;
            int bytesActuallyRead = baseStream.Read(buffer, offset, count);
            readingManyBytes = false;
            if (!readingOneByte) BytesRead += bytesActuallyRead;
            return bytesActuallyRead;
        }


        public override void Write(byte[] buffer, int offset, int count) {
            writingManyBytes = true;
            baseStream.Write(buffer, offset, count);
            writingManyBytes = false;
            if (!writingOneByte) BytesWritten += count;
        }


        public override int ReadByte() {
            readingOneByte = true;
            int value = base.ReadByte();
            readingOneByte = false;
            if (value >= 0 && !readingManyBytes) BytesRead++;
            return value;
        }


        public override void WriteByte(byte value) {
            writingOneByte = true;
            base.WriteByte(value);
            writingOneByte = false;
            if (!writingManyBytes) BytesWritten++;
        }


        public override bool CanRead
        {
            get { return baseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return baseStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return baseStream.CanWrite; }
        }

        public override long Length
        {
            get { return baseStream.Length; }
        }

        public override long Position
        {
            get { return baseStream.Position; }
            set { baseStream.Position = value; }
        }

        public long BytesRead { get; private set; }
        public long BytesWritten { get; private set; }
    }


    /// <summary> Exception thrown when an operation is attempted on an NbtReader that
    /// cannot recover from a previous parsing error. </summary>
    [Serializable]
    public sealed class InvalidReaderStateException : InvalidOperationException {
        internal InvalidReaderStateException(string message)
            : base(message) { }
    }


    /// <summary> BinaryReader wrapper that takes care of reading primitives from an NBT stream,
    /// while taking care of endianness, string encoding, and skipping. </summary>
    internal sealed class NbtBinaryReader : BinaryReader {
        readonly byte[] buffer = new byte[sizeof(double)];

        byte[]? seekBuffer;
        const int SeekBufferSize = 8*1024;
        readonly bool swapNeeded;
        readonly byte[] stringConversionBuffer = new byte[64];


        public NbtBinaryReader(Stream input, bool bigEndian)
            : base(input) {
            swapNeeded = (BitConverter.IsLittleEndian == bigEndian);
        }


        public NbtTagType ReadTagType() {
            int type = ReadByte();
            if (type < 0) {
                throw new EndOfStreamException();
            } else if (type > (int)NbtTagType.IntArray) {
                throw new NbtFormatException("NBT tag type out of range: " + type);
            }

            return (NbtTagType)type;
        }


        public override short ReadInt16() {
            if (swapNeeded) {
                return Swap(base.ReadInt16());
            } else {
                return base.ReadInt16();
            }
        }


        public override int ReadInt32() {
            if (swapNeeded) {
                return Swap(base.ReadInt32());
            } else {
                return base.ReadInt32();
            }
        }


        public override long ReadInt64() {
            if (swapNeeded) {
                return Swap(base.ReadInt64());
            } else {
                return base.ReadInt64();
            }
        }


        public override float ReadSingle() {
            if (swapNeeded) {
                FillBuffer(sizeof(float));
                Array.Reverse(buffer, 0, sizeof(float));
                return BitConverter.ToSingle(buffer, 0);
            } else {
                return base.ReadSingle();
            }
        }


        public override double ReadDouble() {
            if (swapNeeded) {
                FillBuffer(sizeof(double));
                Array.Reverse(buffer);
                return BitConverter.ToDouble(buffer, 0);
            }

            return base.ReadDouble();
        }


        public override string ReadString() {
            short length = ReadInt16();
            if (length < 0) {
                throw new NbtFormatException("Negative string length given!");
            }

            if (length < stringConversionBuffer.Length) {
                int stringBytesRead = 0;
                while (stringBytesRead < length) {
                    int bytesToRead = length - stringBytesRead;
                    int bytesReadThisTime = BaseStream.Read(stringConversionBuffer, stringBytesRead, bytesToRead);
                    if (bytesReadThisTime == 0) {
                        throw new EndOfStreamException();
                    }

                    stringBytesRead += bytesReadThisTime;
                }

                return Encoding.UTF8.GetString(stringConversionBuffer, 0, length);
            } else {
                byte[] stringData = ReadBytes(length);
                if (stringData.Length < length) {
                    throw new EndOfStreamException();
                }

                return Encoding.UTF8.GetString(stringData);
            }
        }


        public void Skip(int bytesToSkip) {
            if (bytesToSkip < 0) {
                throw new ArgumentOutOfRangeException(nameof(bytesToSkip));
            } else if (BaseStream.CanSeek) {
                BaseStream.Position += bytesToSkip;
            } else if (bytesToSkip != 0) {
                if (seekBuffer == null) seekBuffer = new byte[SeekBufferSize];
                int bytesSkipped = 0;
                while (bytesSkipped < bytesToSkip) {
                    int bytesToRead = Math.Min(SeekBufferSize, bytesToSkip - bytesSkipped);
                    int bytesReadThisTime = BaseStream.Read(seekBuffer, 0, bytesToRead);
                    if (bytesReadThisTime == 0) {
                        throw new EndOfStreamException();
                    }

                    bytesSkipped += bytesReadThisTime;
                }
            }
        }


        new void FillBuffer(int numBytes) {
            int offset = 0;
            do {
                int num = BaseStream.Read(buffer, offset, numBytes - offset);
                if (num == 0) throw new EndOfStreamException();
                offset += num;
            } while (offset < numBytes);
        }


        public void SkipString() {
            short length = ReadInt16();
            if (length < 0) {
                throw new NbtFormatException("Negative string length given!");
            }

            Skip(length);
        }


        static short Swap(short v) {
            unchecked {
                return (short)((v >> 8) & 0x00FF |
                               (v << 8) & 0xFF00);
            }
        }


        static int Swap(int v) {
            unchecked {
                uint v2 = (uint)v;
                return (int)((v2 >> 24) & 0x000000FF |
                             (v2 >> 8) & 0x0000FF00 |
                             (v2 << 8) & 0x00FF0000 |
                             (v2 << 24) & 0xFF000000);
            }
        }


        static long Swap(long v) {
            unchecked {
                return (Swap((int)v) & uint.MaxValue) << 32 |
                       Swap((int)(v >> 32)) & uint.MaxValue;
            }
        }


                public TagSelector? Selector { get; set; }
    }


    /// <summary> BinaryWriter wrapper that writes NBT primitives to a stream,
    /// while taking care of endianness and string encoding, and counting bytes written. </summary>
    internal sealed class NbtBinaryWriter {
        // Write at most 4 MiB at a time.
        public const int MaxWriteChunk = 4*1024*1024;

        // Encoding can be shared among all instances of NbtBinaryWriter, because it is stateless.
        static readonly UTF8Encoding Encoding = new UTF8Encoding(false, true);

        // Each instance has to have its own encoder, because it does maintain state.
        readonly Encoder encoder = Encoding.GetEncoder();

        public Stream BaseStream
        {
            get
            {
                stream.Flush();
                return stream;
            }
        }

        readonly Stream stream;

        // Buffer used for temporary conversion
        const int BufferSize = 256;

        // UTF8 characters use at most 4 bytes each.
        const int MaxBufferedStringLength = BufferSize/4;

        // Each NbtBinaryWriter needs to have its own instance of the buffer.
        readonly byte[] buffer = new byte[BufferSize];

        // Swap is only needed if endianness of the runtime differs from desired NBT stream
        readonly bool swapNeeded;


        public NbtBinaryWriter(Stream input, bool bigEndian) {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (!input.CanWrite) throw new ArgumentException("Given stream must be writable", nameof(input));
            stream = input;
            swapNeeded = (BitConverter.IsLittleEndian == bigEndian);
        }


        public void Write(byte value) {
            stream.WriteByte(value);
        }


        public void Write(NbtTagType value) {
            stream.WriteByte((byte)value);
        }


        public void Write(short value) {
            unchecked {
                if (swapNeeded) {
                    buffer[0] = (byte)(value >> 8);
                    buffer[1] = (byte)value;
                } else {
                    buffer[0] = (byte)value;
                    buffer[1] = (byte)(value >> 8);
                }
            }

            stream.Write(buffer, 0, 2);
        }


        public void Write(int value) {
            unchecked {
                if (swapNeeded) {
                    buffer[0] = (byte)(value >> 24);
                    buffer[1] = (byte)(value >> 16);
                    buffer[2] = (byte)(value >> 8);
                    buffer[3] = (byte)value;
                } else {
                    buffer[0] = (byte)value;
                    buffer[1] = (byte)(value >> 8);
                    buffer[2] = (byte)(value >> 16);
                    buffer[3] = (byte)(value >> 24);
                }
            }

            stream.Write(buffer, 0, 4);
        }


        public void Write(long value) {
            unchecked {
                if (swapNeeded) {
                    buffer[0] = (byte)(value >> 56);
                    buffer[1] = (byte)(value >> 48);
                    buffer[2] = (byte)(value >> 40);
                    buffer[3] = (byte)(value >> 32);
                    buffer[4] = (byte)(value >> 24);
                    buffer[5] = (byte)(value >> 16);
                    buffer[6] = (byte)(value >> 8);
                    buffer[7] = (byte)value;
                } else {
                    buffer[0] = (byte)value;
                    buffer[1] = (byte)(value >> 8);
                    buffer[2] = (byte)(value >> 16);
                    buffer[3] = (byte)(value >> 24);
                    buffer[4] = (byte)(value >> 32);
                    buffer[5] = (byte)(value >> 40);
                    buffer[6] = (byte)(value >> 48);
                    buffer[7] = (byte)(value >> 56);
                }
            }

            stream.Write(buffer, 0, 8);
        }


        public void Write(float value) {
            ulong tmpValue = BitConverter.ToUInt32(BitConverter.GetBytes(value));
            unchecked {
                if (swapNeeded) {
                    buffer[0] = (byte)(tmpValue >> 24);
                    buffer[1] = (byte)(tmpValue >> 16);
                    buffer[2] = (byte)(tmpValue >> 8);
                    buffer[3] = (byte)tmpValue;
                } else {
                    buffer[0] = (byte)tmpValue;
                    buffer[1] = (byte)(tmpValue >> 8);
                    buffer[2] = (byte)(tmpValue >> 16);
                    buffer[3] = (byte)(tmpValue >> 24);
                }
            }

            stream.Write(buffer, 0, 4);
        }


        public void Write(double value) {
            ulong tmpValue = BitConverter.ToUInt32(BitConverter.GetBytes(value));
            unchecked {
                if (swapNeeded) {
                    buffer[0] = (byte)(tmpValue >> 56);
                    buffer[1] = (byte)(tmpValue >> 48);
                    buffer[2] = (byte)(tmpValue >> 40);
                    buffer[3] = (byte)(tmpValue >> 32);
                    buffer[4] = (byte)(tmpValue >> 24);
                    buffer[5] = (byte)(tmpValue >> 16);
                    buffer[6] = (byte)(tmpValue >> 8);
                    buffer[7] = (byte)tmpValue;
                } else {
                    buffer[0] = (byte)tmpValue;
                    buffer[1] = (byte)(tmpValue >> 8);
                    buffer[2] = (byte)(tmpValue >> 16);
                    buffer[3] = (byte)(tmpValue >> 24);
                    buffer[4] = (byte)(tmpValue >> 32);
                    buffer[5] = (byte)(tmpValue >> 40);
                    buffer[6] = (byte)(tmpValue >> 48);
                    buffer[7] = (byte)(tmpValue >> 56);
                }
            }

            stream.Write(buffer, 0, 8);
        }


        // Based on BinaryWriter.Write(String)
        public void Write(string value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            // Write out string length (as number of bytes)
            int numBytes = Encoding.GetByteCount(value);
            Write((short)numBytes);

            if (numBytes <= BufferSize) {
                // If the string fits entirely in the buffer, encode and write it as one
                Encoding.GetBytes(value, 0, value.Length, buffer, 0);
                stream.Write(buffer, 0, numBytes);
            } else {
                // Aggressively try to not allocate memory in this loop for runtime performance reasons.
                // Use an Encoder to write out the string correctly (handling surrogates crossing buffer
                // boundaries properly).  
                int charStart = 0;
                int numLeft = value.Length;
                while (numLeft > 0) {
                    // Figure out how many chars to process this round.
                    int charCount = (numLeft > MaxBufferedStringLength) ? MaxBufferedStringLength : numLeft;
                    int byteLen;

                    char[] chars = value.ToCharArray();
                    byte[] bytes = buffer;
                    byteLen = encoder.GetBytes(chars.AsSpan()[charStart..charCount], bytes, charCount == numLeft);

                    // Original unsafe code
                    //fixed (char* pChars = value) {
                    //    fixed (byte* pBytes = buffer) {
                    //        byteLen = encoder.GetBytes(pChars + charStart, charCount, pBytes, BufferSize,
                    //                                   charCount == numLeft);
                    //    }
                    //}

                    stream.Write(buffer, 0, byteLen);
                    charStart += charCount;
                    numLeft -= charCount;
                }
            }
        }


        public void Write(byte[] data, int offset, int count) {
            int written = 0;
            while (written < count) {
                int toWrite = Math.Min(MaxWriteChunk, count - written);
                stream.Write(data, offset + written, toWrite);
                written += toWrite;
            }
        }
    }


    /// <summary> Compression method used for loading/saving NBT files. </summary>
    public enum NbtCompression {
        /// <summary> Automatically detect file compression. Not a valid format for saving. </summary>
        AutoDetect,

        /// <summary> No compression. </summary>
        None,

        /// <summary> Compressed, with GZip header (default). </summary>
        GZip,

        /// <summary> Compressed, with ZLib header (RFC-1950). </summary>
        ZLib
    }


    /// <summary> Represents a complete NBT file. </summary>
    public sealed class NbtFile {
        // Size of buffers that are used to avoid frequent reads from / writes to compressed streams
        const int WriteBufferSize = 8*1024;

        // Size of buffers used for reading to/from files
        const int FileStreamBufferSize = 64*1024;

        /// <summary> Gets the file name used for most recent loading/saving of this file.
        /// May be <c>null</c>, if this <c>NbtFile</c> instance has not been loaded from, or saved to, a file. </summary>
                public string? FileName { get; private set; }

        /// <summary> Gets the compression method used for most recent loading/saving of this file.
        /// Defaults to AutoDetect. </summary>
        public NbtCompression FileCompression { get; private set; }

        /// <summary> Root tag of this file. Must be a named CompoundTag. Defaults to an empty-named tag. </summary>
        /// <exception cref="ArgumentException"> If given tag is unnamed. </exception>
        public NbtCompound RootTag
        {
            get { return rootTag; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value.Name == null) throw new ArgumentException("Root tag must be named.");
                rootTag = value;
            }
        }

        NbtCompound rootTag;

        /// <summary> Whether new NbtFiles should default to big-endian encoding (default: true). </summary>
        public static bool BigEndianByDefault { get; set; }

        /// <summary> Whether this file should read/write tags in big-endian encoding format. </summary>
        public bool BigEndian { get; set; }

        /// <summary> Gets or sets the default value of <c>BufferSize</c> property. Default is 8192. 
        /// Set to 0 to disable buffering by default. </summary>
        /// <exception cref="ArgumentOutOfRangeException"> value is negative. </exception>
        public static int DefaultBufferSize
        {
            get { return defaultBufferSize; }
            set
            {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "DefaultBufferSize cannot be negative.");
                }

                defaultBufferSize = value;
            }
        }

        static int defaultBufferSize = 8*1024;

        /// <summary> Gets or sets the size of internal buffer used for reading files and streams.
        /// Initialized to value of <c>DefaultBufferSize</c> property. </summary>
        /// <exception cref="ArgumentOutOfRangeException"> value is negative. </exception>
        public int BufferSize
        {
            get { return bufferSize; }
            set
            {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "BufferSize cannot be negative.");
                }

                bufferSize = value;
            }
        }

        int bufferSize;


        #region Constructors

        // static constructor
        static NbtFile() {
            BigEndianByDefault = true;
        }


        /// <summary> Creates an empty NbtFile.
        /// RootTag will be set to an empty <c>NbtCompound</c> with a blank name (""). </summary>
        public NbtFile() {
            BigEndian = BigEndianByDefault;
            BufferSize = DefaultBufferSize;
            rootTag = new NbtCompound("");
        }


        /// <summary> Creates a new NBT file with the given root tag. </summary>
        /// <param name="rootTag"> Compound tag to set as the root tag. May be <c>null</c>. </param>
        /// <exception cref="ArgumentException"> If given <paramref name="rootTag"/> is unnamed. </exception>
        public NbtFile(NbtCompound rootTag)
            : this() {
            if (rootTag is null) throw new ArgumentNullException(nameof(rootTag));
            RootTag = rootTag;
        }


        /// <summary> Loads NBT data from a file using the most common settings.
        /// Automatically detects compression. Assumes the file to be big-endian, and uses default buffer size. </summary>
        /// <param name="fileName"> Name of the file from which data will be loaded. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="fileName"/> is <c>null</c>. </exception>
        /// <exception cref="FileNotFoundException"> If given file was not found. </exception>
        /// <exception cref="EndOfStreamException"> If file ended earlier than expected. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected, or decompressing failed. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="IOException"> If an I/O error occurred while reading the file. </exception>
        public NbtFile(string fileName)
            : this() {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            LoadFromFile(fileName, NbtCompression.AutoDetect, null);
        }

        #endregion


        #region Loading

        /// <summary> Loads NBT data from a file. Existing <c>RootTag</c> will be replaced. Compression will be auto-detected. </summary>
        /// <param name="fileName"> Name of the file from which data will be loaded. </param>
        /// <returns> Number of bytes read from the file. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="fileName"/> is <c>null</c>. </exception>
        /// <exception cref="FileNotFoundException"> If given file was not found. </exception>
        /// <exception cref="EndOfStreamException"> If file ended earlier than expected. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected, or decompressing failed. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="IOException"> If an I/O error occurred while reading the file. </exception>
        public long LoadFromFile(string fileName) {
            return LoadFromFile(fileName, NbtCompression.AutoDetect, null);
        }


        /// <summary> Loads NBT data from a file. Existing <c>RootTag</c> will be replaced. </summary>
        /// <param name="fileName"> Name of the file from which data will be loaded. </param>
        /// <param name="compression"> Compression method to use for loading/saving this file. </param>
        /// <param name="selector"> Optional callback to select which tags to load into memory. Root may not be skipped.
        /// No reference is stored to this callback after loading (don't worry about implicitly captured closures). May be <c>null</c>. </param>
        /// <returns> Number of bytes read from the file. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="fileName"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>. </exception>
        /// <exception cref="FileNotFoundException"> If given file was not found. </exception>
        /// <exception cref="EndOfStreamException"> If file ended earlier than expected. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected, or decompressing failed. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="IOException"> If an I/O error occurred while reading the file. </exception>
        public long LoadFromFile(string fileName, NbtCompression compression,
                                 TagSelector? selector) {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            using FileStream? readFileStream = new FileStream(fileName,
                                                    FileMode.Open,
                                                    FileAccess.Read,
                                                    FileShare.Read,
                                                    FileStreamBufferSize,
                                                    FileOptions.SequentialScan);
            LoadFromStream(readFileStream, compression, selector);
            FileName = fileName;
            return readFileStream.Position;
        }


        /// <summary> Loads NBT data from a byte array. Existing <c>RootTag</c> will be replaced. <c>FileName</c> will be set to null. </summary>
        /// <param name="buffer"> Stream from which data will be loaded. If <paramref name="compression"/> is set to AutoDetect, this stream must support seeking. </param>
        /// <param name="index"> The index into <paramref name="buffer"/> at which the stream begins. Must not be negative. </param>
        /// <param name="length"> Maximum number of bytes to read from the given buffer. Must not be negative.
        /// An <see cref="EndOfStreamException"/> is thrown if NBT stream is longer than the given length. </param>
        /// <param name="compression"> Compression method to use for loading/saving this file. </param>
        /// <param name="selector"> Optional callback to select which tags to load into memory. Root may not be skipped.
        /// No reference is stored to this callback after loading (don't worry about implicitly captured closures). May be <c>null</c>. </param>
        /// <returns> Number of bytes read from the buffer. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="buffer"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>;
        /// if <paramref name="index"/> or <paramref name="length"/> is less than zero;
        /// if the sum of <paramref name="index"/> and <paramref name="length"/> is greater than the length of <paramref name="buffer"/>. </exception>
        /// <exception cref="EndOfStreamException"> If NBT stream extends beyond the given <paramref name="length"/>. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected or decompressing failed. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        public long LoadFromBuffer(byte[] buffer, int index, int length, NbtCompression compression,
                                   TagSelector? selector) {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            using MemoryStream? ms = new MemoryStream(buffer, index, length);
            LoadFromStream(ms, compression, selector);
            FileName = null;
            return ms.Position;
        }


        /// <summary> Loads NBT data from a byte array. Existing <c>RootTag</c> will be replaced. <c>FileName</c> will be set to null. </summary>
        /// <param name="buffer"> Stream from which data will be loaded. If <paramref name="compression"/> is set to AutoDetect, this stream must support seeking. </param>
        /// <param name="index"> The index into <paramref name="buffer"/> at which the stream begins. Must not be negative. </param>
        /// <param name="length"> Maximum number of bytes to read from the given buffer. Must not be negative.
        /// An <see cref="EndOfStreamException"/> is thrown if NBT stream is longer than the given length. </param>
        /// <param name="compression"> Compression method to use for loading/saving this file. </param>
        /// <returns> Number of bytes read from the buffer. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="buffer"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>;
        /// if <paramref name="index"/> or <paramref name="length"/> is less than zero;
        /// if the sum of <paramref name="index"/> and <paramref name="length"/> is greater than the length of <paramref name="buffer"/>. </exception>
        /// <exception cref="EndOfStreamException"> If NBT stream extends beyond the given <paramref name="length"/>. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected or decompressing failed. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        public long LoadFromBuffer(byte[] buffer, int index, int length, NbtCompression compression) {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            using MemoryStream? ms = new MemoryStream(buffer, index, length);
            LoadFromStream(ms, compression, null);
            FileName = null;
            return ms.Position;
        }


        /// <summary> Loads NBT data from a stream. Existing <c>RootTag</c> will be replaced </summary>
        /// <param name="stream"> Stream from which data will be loaded. If compression is set to AutoDetect, this stream must support seeking. </param>
        /// <param name="compression"> Compression method to use for loading/saving this file. </param>
        /// <param name="selector"> Optional callback to select which tags to load into memory. Root may not be skipped.
        /// No reference is stored to this callback after loading (don't worry about implicitly captured closures). May be <c>null</c>. </param>
        /// <returns> Number of bytes read from the stream. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>. </exception>
        /// <exception cref="NotSupportedException"> If <paramref name="compression"/> is set to AutoDetect, but the stream is not seekable. </exception>
        /// <exception cref="EndOfStreamException"> If file ended earlier than expected. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected, decompressing failed, or given stream does not support reading. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        public long LoadFromStream(Stream stream, NbtCompression compression,
                                   TagSelector? selector) {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            FileName = null;

            // detect compression, based on the first byte
            if (compression == NbtCompression.AutoDetect) {
                FileCompression = DetectCompression(stream);
            } else {
                FileCompression = compression;
            }

            // prepare to count bytes read
            long startOffset = 0;
            if (stream.CanSeek) {
                startOffset = stream.Position;
            } else {
                stream = new ByteCountingStream(stream);
            }

            switch (FileCompression) {
                case NbtCompression.GZip:
                    using (GZipStream? decStream = new GZipStream(stream, CompressionMode.Decompress, true)) {
                        if (bufferSize > 0) {
                            LoadFromStreamInternal(new BufferedStream(decStream, bufferSize), selector);
                        } else {
                            LoadFromStreamInternal(decStream, selector);
                        }
                    }

                    break;

                case NbtCompression.None:
                    LoadFromStreamInternal(stream, selector);
                    break;

                case NbtCompression.ZLib:
                    if (stream.ReadByte() != 0x78) {
                        throw new InvalidDataException(WrongZLibHeaderMessage);
                    }

                    stream.ReadByte();
                    using (DeflateStream? decStream = new DeflateStream(stream, CompressionMode.Decompress, true)) {
                        if (bufferSize > 0) {
                            LoadFromStreamInternal(new BufferedStream(decStream, bufferSize), selector);
                        } else {
                            LoadFromStreamInternal(decStream, selector);
                        }
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(compression));
            }

            // report bytes read
            if (stream.CanSeek) {
                return stream.Position - startOffset;
            } else {
                return ((ByteCountingStream)stream).BytesRead;
            }
        }


        /// <summary> Loads NBT data from a stream. Existing <c>RootTag</c> will be replaced </summary>
        /// <param name="stream"> Stream from which data will be loaded. If compression is set to AutoDetect, this stream must support seeking. </param>
        /// <param name="compression"> Compression method to use for loading/saving this file. </param>
        /// <returns> Number of bytes read from the stream. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>. </exception>
        /// <exception cref="NotSupportedException"> If <paramref name="compression"/> is set to AutoDetect, but the stream is not seekable. </exception>
        /// <exception cref="EndOfStreamException"> If file ended earlier than expected. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected, decompressing failed, or given stream does not support reading. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        public long LoadFromStream(Stream stream, NbtCompression compression) {
            return LoadFromStream(stream, compression, null);
        }


        static NbtCompression DetectCompression(Stream stream) {
            if (!stream.CanSeek) {
                throw new NotSupportedException("Cannot auto-detect compression on a stream that's not seekable.");
            }

            int firstByte = stream.ReadByte();
            var compression = firstByte switch
            {
                -1 => throw new EndOfStreamException(),
                // 0x0A
                (byte)NbtTagType.Compound => NbtCompression.None,
                0x1F => NbtCompression.GZip,// GZip magic number
                0x78 => NbtCompression.ZLib,// ZLib header
                _ => throw new InvalidDataException("Could not auto-detect compression format."),
            };
            stream.Seek(-1, SeekOrigin.Current);
            return compression;
        }


        void LoadFromStreamInternal(Stream stream, TagSelector? tagSelector) {
            // Make sure the first byte in this file is the tag for a TAG_Compound
            int firstByte = stream.ReadByte();
            if (firstByte < 0) {
                throw new EndOfStreamException();
            }

            if (firstByte != (int)NbtTagType.Compound) {
                throw new NbtFormatException("Given NBT stream does not start with a TAG_Compound");
            }

            NbtBinaryReader? reader = new NbtBinaryReader(stream, BigEndian) {
                Selector = tagSelector
            };

            NbtCompound? rootCompound = new NbtCompound(reader.ReadString());
            rootCompound.ReadTag(reader);
            RootTag = rootCompound;
        }

        #endregion


        #region Saving

        /// <summary> Saves this NBT file to a stream. Nothing is written to stream if RootTag is <c>null</c>. </summary>
        /// <param name="fileName"> File to write data to. May not be <c>null</c>. </param>
        /// <param name="compression"> Compression mode to use for saving. May not be AutoDetect. </param>
        /// <returns> Number of bytes written to the file. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="fileName"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If AutoDetect was given as the <paramref name="compression"/> mode. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>. </exception>
        /// <exception cref="InvalidDataException"> If given stream does not support writing. </exception>
        /// <exception cref="IOException"> If an I/O error occurred while creating the file. </exception>
        /// <exception cref="UnauthorizedAccessException"> Specified file is read-only, or a permission issue occurred. </exception>
        /// <exception cref="NbtFormatException"> If one of the NbtCompound tags contained unnamed tags;
        /// or if an NbtList tag had Unknown list type and no elements. </exception>
        public long SaveToFile(string fileName, NbtCompression compression) {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            using FileStream? saveFile = new FileStream(fileName,
                                              FileMode.Create,
                                              FileAccess.Write,
                                              FileShare.None,
                                              FileStreamBufferSize,
                                              FileOptions.SequentialScan);
            return SaveToStream(saveFile, compression);
        }


        /// <summary> Saves this NBT file to a stream. Nothing is written to stream if RootTag is <c>null</c>. </summary>
        /// <param name="buffer"> Buffer to write data to. May not be <c>null</c>. </param>
        /// <param name="index"> The index into <paramref name="buffer"/> at which the stream should begin. </param>
        /// <param name="compression"> Compression mode to use for saving. May not be AutoDetect. </param>
        /// <returns> Number of bytes written to the buffer. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="buffer"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If AutoDetect was given as the <paramref name="compression"/> mode. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>;
        /// if <paramref name="index"/> is less than zero; or if <paramref name="index"/> is greater than the length of <paramref name="buffer"/>. </exception>
        /// <exception cref="InvalidDataException"> If given stream does not support writing. </exception>
        /// <exception cref="UnauthorizedAccessException"> Specified file is read-only, or a permission issue occurred. </exception>
        /// <exception cref="NbtFormatException"> If one of the NbtCompound tags contained unnamed tags;
        /// or if an NbtList tag had Unknown list type and no elements. </exception>
        public long SaveToBuffer(byte[] buffer, int index, NbtCompression compression) {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            using MemoryStream? ms = new MemoryStream(buffer, index, buffer.Length - index);
            return SaveToStream(ms, compression);
        }


        /// <summary> Saves this NBT file to a stream. Nothing is written to stream if RootTag is <c>null</c>. </summary>
        /// <param name="compression"> Compression mode to use for saving. May not be AutoDetect. </param>
        /// <returns> Byte array containing the serialized NBT data. </returns>
        /// <exception cref="ArgumentException"> If AutoDetect was given as the <paramref name="compression"/> mode. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>. </exception>
        /// <exception cref="InvalidDataException"> If given stream does not support writing. </exception>
        /// <exception cref="UnauthorizedAccessException"> Specified file is read-only, or a permission issue occurred. </exception>
        /// <exception cref="NbtFormatException"> If one of the NbtCompound tags contained unnamed tags;
        /// or if an NbtList tag had Unknown list type and no elements. </exception>
        public byte[] SaveToBuffer(NbtCompression compression) {
            using MemoryStream? ms = new MemoryStream();
            SaveToStream(ms, compression);
            return ms.ToArray();
        }


        /// <summary> Saves this NBT file to a stream. Nothing is written to stream if RootTag is <c>null</c>. </summary>
        /// <param name="stream"> Stream to write data to. May not be <c>null</c>. </param>
        /// <param name="compression"> Compression mode to use for saving. May not be AutoDetect. </param>
        /// <returns> Number of bytes written to the stream. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> If AutoDetect was given as the <paramref name="compression"/> mode. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>. </exception>
        /// <exception cref="InvalidDataException"> If given stream does not support writing. </exception>
        /// <exception cref="NbtFormatException"> If RootTag is null;
        /// or if RootTag is unnamed;
        /// or if one of the NbtCompound tags contained unnamed tags;
        /// or if an NbtList tag had Unknown list type and no elements. </exception>
        public long SaveToStream(Stream stream, NbtCompression compression) {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            switch (compression) {
                case NbtCompression.AutoDetect:
                    throw new ArgumentException("AutoDetect is not a valid NbtCompression value for saving.");
                case NbtCompression.ZLib:
                case NbtCompression.GZip:
                case NbtCompression.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(compression));
            }

            if (rootTag.Name == null) {
                // This may trigger if root tag has been renamed
                throw new NbtFormatException(
                    "Cannot save NbtFile: Root tag is not named. Its name may be an empty string, but not null.");
            }

            long startOffset = 0;
            if (stream.CanSeek) {
                startOffset = stream.Position;
            } else {
                stream = new ByteCountingStream(stream);
            }

            switch (compression) {
                case NbtCompression.ZLib:
                    stream.WriteByte(0x78);
                    stream.WriteByte(0x01);
                    int checksum;
                    using (ZLibStream? compressStream = new ZLibStream(stream, CompressionMode.Compress, true)) {
                        BufferedStream? bufferedStream = new BufferedStream(compressStream, WriteBufferSize);
                        RootTag.WriteTag(new NbtBinaryWriter(bufferedStream, BigEndian));
                        bufferedStream.Flush();
                        checksum = compressStream.Checksum;
                    }

                    byte[] checksumBytes = BitConverter.GetBytes(checksum);
                    if (BitConverter.IsLittleEndian) {
                        // Adler32 checksum is big-endian
                        Array.Reverse(checksumBytes);
                    }

                    stream.Write(checksumBytes, 0, checksumBytes.Length);
                    break;

                case NbtCompression.GZip:
                    using (GZipStream? compressStream = new GZipStream(stream, CompressionMode.Compress, true)) {
                        // use a buffered stream to avoid GZipping in small increments (which has a lot of overhead)
                        BufferedStream? bufferedStream = new BufferedStream(compressStream, WriteBufferSize);
                        RootTag.WriteTag(new NbtBinaryWriter(bufferedStream, BigEndian));
                        bufferedStream.Flush();
                    }

                    break;

                case NbtCompression.None:
                    NbtBinaryWriter? writer = new NbtBinaryWriter(stream, BigEndian);
                    RootTag.WriteTag(writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(compression));
            }

            if (stream.CanSeek) {
                return stream.Position - startOffset;
            } else {
                return ((ByteCountingStream)stream).BytesWritten;
            }
        }

        #endregion


        /// <summary> Reads the root name from the given NBT file. Automatically detects compression. </summary>
        /// <param name="fileName"> Name of the file from which first tag will be read. </param>
        /// <returns> Name of the root tag in the given NBT file. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="fileName"/> is <c>null</c>. </exception>
        /// <exception cref="FileNotFoundException"> If given file was not found. </exception>
        /// <exception cref="EndOfStreamException"> If file ended earlier than expected. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected, or decompressing failed. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="IOException"> If an I/O error occurred while reading the file. </exception>
        public static string ReadRootTagName(string fileName) {
            return ReadRootTagName(fileName, NbtCompression.AutoDetect, BigEndianByDefault, defaultBufferSize);
        }


        /// <summary> Reads the root name from the given NBT file. </summary>
        /// <param name="fileName"> Name of the file from which data will be loaded. </param>
        /// <param name="compression"> Format in which the given file is compressed. </param>
        /// <param name="bigEndian"> Whether the file uses big-endian (default) or little-endian encoding. </param>
        /// <param name="bufferSize"> Buffer size to use for reading, in bytes. Default is 8192. </param>
        /// <returns> Name of the root tag in the given NBT file. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="fileName"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>. </exception>
        /// <exception cref="FileNotFoundException"> If given file was not found. </exception>
        /// <exception cref="EndOfStreamException"> If file ended earlier than expected. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected, or decompressing failed. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="IOException"> If an I/O error occurred while reading the file. </exception>
        public static string ReadRootTagName(string fileName, NbtCompression compression, bool bigEndian,
                                             int bufferSize) {
            if (fileName == null) {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (!File.Exists(fileName)) {
                throw new FileNotFoundException("Could not find the given NBT file.", fileName);
            }

            if (bufferSize < 0) {
                throw new ArgumentOutOfRangeException(nameof(bufferSize), bufferSize,
                                                      "DefaultBufferSize cannot be negative.");
            }

            using FileStream readFileStream = File.OpenRead(fileName);
            return ReadRootTagName(readFileStream, compression, bigEndian, bufferSize);
        }


        /// <summary> Reads the root name from the given stream of NBT data. </summary>
        /// <param name="stream"> Stream from which data will be loaded. If compression is set to AutoDetect, this stream must support seeking. </param>
        /// <param name="compression"> Compression method to use for loading this stream. </param>
        /// <param name="bigEndian"> Whether the stream uses big-endian (default) or little-endian encoding. </param>
        /// <param name="bufferSize"> Buffer size to use for reading, in bytes. Default is 8192. </param>
        /// <returns> Name of the root tag in the given stream. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> If an unrecognized/unsupported value was given for <paramref name="compression"/>. </exception>
        /// <exception cref="NotSupportedException"> If compression is set to AutoDetect, but the stream is not seekable. </exception>
        /// <exception cref="EndOfStreamException"> If file ended earlier than expected. </exception>
        /// <exception cref="InvalidDataException"> If file compression could not be detected, decompressing failed, or given stream does not support reading. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        public static string ReadRootTagName(Stream stream, NbtCompression compression, bool bigEndian,
                                             int bufferSize) {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (bufferSize < 0) {
                throw new ArgumentOutOfRangeException(nameof(bufferSize), bufferSize,
                                                      "DefaultBufferSize cannot be negative.");
            }

            // detect compression, based on the first byte
            if (compression == NbtCompression.AutoDetect) {
                compression = DetectCompression(stream);
            }

            switch (compression) {
                case NbtCompression.GZip:
                    using (GZipStream? decStream = new GZipStream(stream, CompressionMode.Decompress, true)) {
                        if (bufferSize > 0) {
                            return GetRootNameInternal(new BufferedStream(decStream, bufferSize), bigEndian);
                        } else {
                            return GetRootNameInternal(decStream, bigEndian);
                        }
                    }

                case NbtCompression.None:
                    return GetRootNameInternal(stream, bigEndian);

                case NbtCompression.ZLib:
                    if (stream.ReadByte() != 0x78) {
                        throw new InvalidDataException(WrongZLibHeaderMessage);
                    }

                    stream.ReadByte();
                    using (DeflateStream? decStream = new DeflateStream(stream, CompressionMode.Decompress, true)) {
                        if (bufferSize > 0) {
                            return GetRootNameInternal(new BufferedStream(decStream, bufferSize), bigEndian);
                        } else {
                            return GetRootNameInternal(decStream, bigEndian);
                        }
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(compression));
            }
        }


        static string GetRootNameInternal(Stream stream, bool bigEndian) {
            int firstByte = stream.ReadByte();
            if (firstByte < 0) {
                throw new EndOfStreamException();
            } else if (firstByte != (int)NbtTagType.Compound) {
                throw new NbtFormatException("Given NBT stream does not start with a TAG_Compound");
            }

            NbtBinaryReader? reader = new NbtBinaryReader(stream, bigEndian);

            return reader.ReadString();
        }


        /// <summary> Prints contents of the root tag, and any child tags, to a string. </summary>
        public override string ToString() {
            return RootTag.ToString(NbtTag.DefaultIndentString);
        }


        /// <summary> Prints contents of the root tag, and any child tags, to a string.
        /// Indents the string using multiples of the given indentation string. </summary>
        /// <param name="indentString"> String to be used for indentation. </param>
        /// <returns> A string representing contents of this tag, and all child tags (if any). </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="indentString"/> is <c>null</c>. </exception>
        public string ToString(string indentString) {
            return RootTag.ToString(indentString);
        }


        const string WrongZLibHeaderMessage = "Unrecognized ZLib header. Expected 0x78";
    }


    /// <summary> Exception thrown when a format violation is detected while
    /// parsing or serializing an NBT file. </summary>
    [Serializable]
    public sealed class NbtFormatException : Exception {
        internal NbtFormatException(string message)
            : base(message) { }
    }


    internal enum NbtParseState {
        AtStreamBeginning,
        AtCompoundBeginning,
        InCompound,
        AtCompoundEnd,
        AtListBeginning,
        InList,
        AtStreamEnd,
        Error
    }


    /// <summary> Represents a reader that provides fast, non-cached, forward-only access to NBT data.
    /// Each instance of NbtReader reads one complete file. </summary>
    public class NbtReader {
        NbtParseState state = NbtParseState.AtStreamBeginning;
        readonly NbtBinaryReader reader;
        Stack<NbtReaderNode>? nodes;
        readonly long streamStartOffset;
        bool atValue;
        object? valueCache;
        readonly bool canSeekStream;


        /// <summary> Initializes a new instance of the NbtReader class. </summary>
        /// <param name="stream"> Stream to read from. </param>
        /// <remarks> Assumes that data in the stream is Big-Endian encoded. </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> <paramref name="stream"/> is not readable. </exception>
        public NbtReader(Stream stream)
            : this(stream, true) { }


        /// <summary> Initializes a new instance of the NbtReader class. </summary>
        /// <param name="stream"> Stream to read from. </param>
        /// <param name="bigEndian"> Whether NBT data is in Big-Endian encoding. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> <paramref name="stream"/> is not readable. </exception>
        public NbtReader(Stream stream, bool bigEndian) {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            SkipEndTags = true;
            CacheTagValues = false;
            ParentTagType = NbtTagType.Unknown;
            TagType = NbtTagType.Unknown;

            canSeekStream = stream.CanSeek;
            if (canSeekStream) {
                streamStartOffset = stream.Position;
            }

            reader = new NbtBinaryReader(stream, bigEndian);
        }


        /// <summary> Gets the name of the root tag of this NBT stream. </summary>
                public string? RootName { get; private set; }

        /// <summary> Gets the name of the parent tag. May be null (for root tags and descendants of list elements). </summary>
                public string? ParentName { get; private set; }

        /// <summary> Gets the name of the current tag. May be null (for list elements and end tags). </summary>
                public string? TagName { get; private set; }

        /// <summary> Gets the type of the parent tag. Returns TagType.Unknown if there is no parent tag. </summary>
        public NbtTagType ParentTagType { get; private set; }

        /// <summary> Gets the type of the current tag. </summary>
        public NbtTagType TagType { get; private set; }

        /// <summary> Whether tag that we are currently on is a list element. </summary>
        public bool IsListElement
        {
            get { return (ParentTagType == NbtTagType.List); }
        }

        /// <summary> Whether current tag has a value to read. </summary>
        public bool HasValue
        {
            get
            {
                return TagType switch
                {
                    NbtTagType.Compound or NbtTagType.End or NbtTagType.List or NbtTagType.Unknown => false,
                    _ => true,
                };
            }
        }

        /// <summary> Whether current tag has a name. </summary>
        public bool HasName
        {
            get { return (TagName != null); }
        }

        /// <summary> Whether this reader has reached the end of stream. </summary>
        public bool IsAtStreamEnd
        {
            get { return state == NbtParseState.AtStreamEnd; }
        }

        /// <summary> Whether the current tag is a Compound. </summary>
        public bool IsCompound
        {
            get { return (TagType == NbtTagType.Compound); }
        }

        /// <summary> Whether the current tag is a List. </summary>
        public bool IsList
        {
            get { return (TagType == NbtTagType.List); }
        }

        /// <summary> Whether the current tag has length (Lists, ByteArrays, and IntArrays have length).
        /// Compound tags also have length, technically, but it is not known until all child tags are read. </summary>
        public bool HasLength
        {
            get
            {
                return TagType switch
                {
                    NbtTagType.List or NbtTagType.ByteArray or NbtTagType.IntArray => true,
                    _ => false,
                };
            }
        }

        /// <summary> Gets the Stream from which data is being read. </summary>
        public Stream BaseStream
        {
            get { return reader.BaseStream; }
        }

        /// <summary> Gets the number of bytes from the beginning of the stream to the beginning of this tag.
        /// If the stream is not seekable, this value will always be 0. </summary>
        public int TagStartOffset { get; private set; }

        /// <summary> Gets the number of tags read from the stream so far
        /// (including the current tag and all skipped tags). 
        /// If <c>SkipEndTags</c> is <c>false</c>, all end tags are also counted. </summary>
        public int TagsRead { get; private set; }

        /// <summary> Gets the depth of the current tag in the hierarchy.
        /// <c>RootTag</c> is at depth 1, its descendant tags are 2, etc. </summary>
        public int Depth { get; private set; }

        /// <summary> If the current tag is TAG_List, returns type of the list elements. </summary>
        public NbtTagType ListType { get; private set; }

        /// <summary> If the current tag is TAG_List, TAG_Byte_Array, or TAG_Int_Array, returns the number of elements. </summary>
        public int TagLength { get; private set; }

        /// <summary> If the parent tag is TAG_List, returns the number of elements. </summary>
        public int ParentTagLength { get; private set; }

        /// <summary> If the parent tag is TAG_List, returns index of the current tag. </summary>
        public int ListIndex { get; private set; }

        /// <summary> Gets whether this NbtReader instance is in state of error.
        /// No further reading can be done from this instance if a parse error occurred. </summary>
        public bool IsInErrorState
        {
            get { return (state == NbtParseState.Error); }
        }


        /// <summary> Reads the next tag from the stream. </summary>
        /// <returns> true if the next tag was read successfully; false if there are no more tags to read. </returns>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="InvalidReaderStateException"> If NbtReader cannot recover from a previous parsing error. </exception>
        public bool ReadToFollowing() {
            switch (state) {
                case NbtParseState.AtStreamBeginning:
                    // set state to error in case reader.ReadTagType throws.
                    state = NbtParseState.Error;
                    // read first tag, make sure it's a compound
                    if (reader.ReadTagType() != NbtTagType.Compound) {
                        state = NbtParseState.Error;
                        throw new NbtFormatException("Given NBT stream does not start with a TAG_Compound");
                    }

                    Depth = 1;
                    TagType = NbtTagType.Compound;
                    // Read root name. Advance to the first inside tag.
                    ReadTagHeader(true);
                    RootName = TagName;
                    return true;

                case NbtParseState.AtCompoundBeginning:
                    GoDown();
                    state = NbtParseState.InCompound;
                    goto case NbtParseState.InCompound;

                case NbtParseState.InCompound:
                    if (atValue) {
                        SkipValue();
                    }

                    // Read next tag, check if we've hit the end
                    if (canSeekStream) {
                        TagStartOffset = (int)(reader.BaseStream.Position - streamStartOffset);
                    }

                    NbtParseState oldState = state;
                    // set state to error in case reader.ReadTagType throws.
                    state = NbtParseState.Error;
                    TagType = reader.ReadTagType();
                    state = oldState;

                    if (TagType == NbtTagType.End) {
                        TagName = null;
                        TagsRead++;
                        state = NbtParseState.AtCompoundEnd;
                        if (SkipEndTags) {
                            TagsRead--;
                            goto case NbtParseState.AtCompoundEnd;
                        } else {
                            return true;
                        }
                    } else {
                        ReadTagHeader(true);
                        return true;
                    }

                case NbtParseState.AtListBeginning:
                    GoDown();
                    ListIndex = -1;
                    TagType = ListType;
                    state = NbtParseState.InList;
                    goto case NbtParseState.InList;

                case NbtParseState.InList:
                    if (atValue) {
                        SkipValue();
                    }

                    ListIndex++;
                    if (ListIndex >= ParentTagLength) {
                        GoUp();
                        if (ParentTagType == NbtTagType.List) {
                            state = NbtParseState.InList;
                            TagType = NbtTagType.List;
                            goto case NbtParseState.InList;
                        } else if (ParentTagType == NbtTagType.Compound) {
                            state = NbtParseState.InCompound;
                            goto case NbtParseState.InCompound;
                        } else {
                            // This should not happen unless NbtReader is bugged
                            state = NbtParseState.Error;
                            throw new NbtFormatException(InvalidParentTagError);
                        }
                    } else {
                        if (canSeekStream) {
                            TagStartOffset = (int)(reader.BaseStream.Position - streamStartOffset);
                        }

                        ReadTagHeader(false);
                    }

                    return true;

                case NbtParseState.AtCompoundEnd:
                    GoUp();
                    if (ParentTagType == NbtTagType.List) {
                        state = NbtParseState.InList;
                        TagType = NbtTagType.Compound;
                        goto case NbtParseState.InList;
                    } else if (ParentTagType == NbtTagType.Compound) {
                        state = NbtParseState.InCompound;
                        goto case NbtParseState.InCompound;
                    } else if (ParentTagType == NbtTagType.Unknown) {
                        state = NbtParseState.AtStreamEnd;
                        return false;
                    } else {
                        // This should not happen unless NbtReader is bugged
                        state = NbtParseState.Error;
                        throw new NbtFormatException(InvalidParentTagError);
                    }

                case NbtParseState.AtStreamEnd:
                    // nothing left to read!
                    return false;

                default:
                    // Parsing error, or unexpected state.
                    throw new InvalidReaderStateException(ErroneousStateError);
            }
        }


        void ReadTagHeader(bool readName) {
            TagsRead++;
            TagName = (readName ? reader.ReadString() : null);

            valueCache = null;
            TagLength = 0;
            atValue = false;
            ListType = NbtTagType.Unknown;

            switch (TagType) {
                case NbtTagType.Byte:
                case NbtTagType.Short:
                case NbtTagType.Int:
                case NbtTagType.Long:
                case NbtTagType.Float:
                case NbtTagType.Double:
                case NbtTagType.String:
                    atValue = true;
                    break;

                case NbtTagType.IntArray:
                case NbtTagType.ByteArray:
                    TagLength = reader.ReadInt32();
                    atValue = true;
                    break;

                case NbtTagType.List:
                    // Setting state to error in case reader throws
                    state = NbtParseState.Error;
                    ListType = reader.ReadTagType();
                    TagLength = reader.ReadInt32();
                    if (TagLength < 0) {
                        throw new NbtFormatException("Negative tag length given: " + TagLength);
                    }

                    state = NbtParseState.AtListBeginning;
                    break;

                case NbtTagType.Compound:
                    state = NbtParseState.AtCompoundBeginning;
                    break;

                default:
                    state = NbtParseState.Error;
                    throw new NbtFormatException("Trying to read tag of unknown type.");
            }
        }


        // Goes one step down the NBT file's hierarchy, preserving current state
        void GoDown() {
            if (nodes == null) nodes = new Stack<NbtReaderNode>();
            NbtReaderNode? newNode = new NbtReaderNode {
                ListIndex = ListIndex,
                ParentTagLength = ParentTagLength,
                ParentName = ParentName,
                ParentTagType = ParentTagType,
                ListType = ListType
            };
            nodes.Push(newNode);

            ParentName = TagName;
            ParentTagType = TagType;
            ParentTagLength = TagLength;
            ListIndex = 0;
            TagLength = 0;

            Depth++;
        }


        // Goes one step up the NBT file's hierarchy, restoring previous state
        void GoUp() {
            if (nodes is null) return;
            NbtReaderNode oldNode = nodes.Pop();

            ParentName = oldNode.ParentName;
            ParentTagType = oldNode.ParentTagType;
            ParentTagLength = oldNode.ParentTagLength;
            ListIndex = oldNode.ListIndex;
            ListType = oldNode.ListType;
            TagLength = 0;

            Depth--;
        }


        void SkipValue() {
            // Make sure to check for "atValue" before calling this method
            switch (TagType) {
                case NbtTagType.Byte:
                    reader.ReadByte();
                    break;

                case NbtTagType.Short:
                    reader.ReadInt16();
                    break;

                case NbtTagType.Float:
                case NbtTagType.Int:
                    reader.ReadInt32();
                    break;

                case NbtTagType.Double:
                case NbtTagType.Long:
                    reader.ReadInt64();
                    break;

                case NbtTagType.ByteArray:
                    reader.Skip(TagLength);
                    break;

                case NbtTagType.IntArray:
                    reader.Skip(sizeof(int)*TagLength);
                    break;

                case NbtTagType.String:
                    reader.SkipString();
                    break;

                default:
                    throw new InvalidOperationException(NonValueTagError);
            }

            atValue = false;
            valueCache = null;
        }


        /// <summary> Reads until a tag with the specified name is found. 
        /// Returns false if are no more tags to read (end of stream is reached). </summary>
        /// <param name="tagName"> Name of the tag. May be null (to look for next unnamed tag). </param>
        /// <returns> <c>true</c> if a matching tag is found; otherwise <c>false</c>. </returns>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="InvalidOperationException"> If NbtReader cannot recover from a previous parsing error. </exception>
        public bool ReadToFollowing(string? tagName) {
            while (ReadToFollowing()) {
                if (TagName == tagName) {
                    return true;
                }
            }

            return false;
        }


        /// <summary> Advances the NbtReader to the next descendant tag with the specified name.
        /// If a matching child tag is not found, the NbtReader is positioned on the end tag. </summary>
        /// <param name="tagName"> Name of the tag you wish to move to. May be null (to look for next unnamed tag). </param>
        /// <returns> <c>true</c> if a matching descendant tag is found; otherwise <c>false</c>. </returns>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="InvalidReaderStateException"> If NbtReader cannot recover from a previous parsing error. </exception>
        public bool ReadToDescendant(string? tagName) {
            if (state == NbtParseState.Error) {
                throw new InvalidReaderStateException(ErroneousStateError);
            } else if (state == NbtParseState.AtStreamEnd) {
                return false;
            }

            int currentDepth = Depth;
            while (ReadToFollowing()) {
                if (Depth <= currentDepth) {
                    return false;
                } else if (TagName == tagName) {
                    return true;
                }
            }

            return false;
        }


        /// <summary> Advances the NbtReader to the next sibling tag, skipping any child tags.
        /// If there are no more siblings, NbtReader is positioned on the tag following the last of this tag's descendants. </summary>
        /// <returns> <c>true</c> if a sibling element is found; otherwise <c>false</c>. </returns>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="InvalidReaderStateException"> If NbtReader cannot recover from a previous parsing error. </exception>
        public bool ReadToNextSibling() {
            if (state == NbtParseState.Error) {
                throw new InvalidReaderStateException(ErroneousStateError);
            } else if (state == NbtParseState.AtStreamEnd) {
                return false;
            }

            int currentDepth = Depth;
            while (ReadToFollowing()) {
                if (Depth == currentDepth) {
                    return true;
                } else if (Depth < currentDepth) {
                    return false;
                }
            }

            return false;
        }


        /// <summary> Advances the NbtReader to the next sibling tag with the specified name.
        /// If a matching sibling tag is not found, NbtReader is positioned on the tag following the last siblings. </summary>
        /// <param name="tagName"> The name of the sibling tag you wish to move to. </param>
        /// <returns> <c>true</c> if a matching sibling element is found; otherwise <c>false</c>. </returns>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="InvalidOperationException"> If NbtReader cannot recover from a previous parsing error. </exception>
        public bool ReadToNextSibling(string? tagName) {
            while (ReadToNextSibling()) {
                if (TagName == tagName) {
                    return true;
                }
            }

            return false;
        }


        /// <summary> Skips current tag, its value/descendants, and any following siblings.
        /// In other words, reads until parent tag's sibling. </summary>
        /// <returns> Total number of tags that were skipped. Returns 0 if end of the stream is reached. </returns>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="InvalidReaderStateException"> If NbtReader cannot recover from a previous parsing error. </exception>
        public int Skip() {
            if (state == NbtParseState.Error) {
                throw new InvalidReaderStateException(ErroneousStateError);
            } else if (state == NbtParseState.AtStreamEnd) {
                return 0;
            }

            int startDepth = Depth;
            int skipped = 0;
            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (ReadToFollowing() && Depth >= startDepth) {
                skipped++;
            }

            return skipped;
        }


        /// <summary> Reads the entirety of the current tag, including any descendants,
        /// and constructs an NbtTag object of the appropriate type. </summary>
        /// <returns> Constructed NbtTag object;
        /// <c>null</c> if <c>SkipEndTags</c> is <c>true</c> and trying to read an End tag. </returns>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="InvalidReaderStateException"> If NbtReader cannot recover from a previous parsing error. </exception>
        /// <exception cref="EndOfStreamException"> End of stream has been reached (no more tags can be read). </exception>
        /// <exception cref="InvalidOperationException"> Tag value has already been read, and CacheTagValues is false. </exception>
        public NbtTag ReadAsTag() {
            switch (state) {
                case NbtParseState.Error:
                    throw new InvalidReaderStateException(ErroneousStateError);

                case NbtParseState.AtStreamEnd:
                    throw new EndOfStreamException();

                case NbtParseState.AtStreamBeginning:
                case NbtParseState.AtCompoundEnd:
                    ReadToFollowing();
                    break;
            }

            // get this tag
            NbtTag parent;
            if (TagType == NbtTagType.Compound) {
                parent = new NbtCompound(TagName);
            } else if (TagType == NbtTagType.List) {
                parent = new NbtList(TagName, ListType);
            } else if (atValue) {
                NbtTag result = ReadValueAsTag();
                ReadToFollowing();
                // if we're at a value tag, there are no child tags to read
                return result;
            } else {
                // end tags cannot be read-as-tags (there is no corresponding NbtTag object)
                throw new InvalidOperationException(NoValueToReadError);
            }

            int startingDepth = Depth;
            int parentDepth = Depth;

            do {
                ReadToFollowing();
                // Going up the file tree, or end of document: wrap up
                while (Depth <= parentDepth && parent.Parent != null) {
                    parent = parent.Parent;
                    parentDepth--;
                }

                if (Depth <= startingDepth) break;

                NbtTag thisTag;
                if (TagType == NbtTagType.Compound) {
                    thisTag = new NbtCompound(TagName);
                    AddToParent(thisTag, parent);
                    parent = thisTag;
                    parentDepth = Depth;
                } else if (TagType == NbtTagType.List) {
                    thisTag = new NbtList(TagName, ListType);
                    AddToParent(thisTag, parent);
                    parent = thisTag;
                    parentDepth = Depth;
                } else if (TagType != NbtTagType.End) {
                    thisTag = ReadValueAsTag();
                    AddToParent(thisTag, parent);
                }
            } while (true);

            return parent;
        }


        static void AddToParent(NbtTag thisTag, NbtTag parent) {
            NbtList? parentAsList = parent as NbtList;
            if (parentAsList is not null) {
                parentAsList.Add(thisTag);
            } else {
                NbtCompound? parentAsCompound = parent as NbtCompound;
                if (parentAsCompound is not null) {
                    parentAsCompound.Add(thisTag);
                } else {
                    // cannot happen unless NbtReader is bugged
                    throw new NbtFormatException(InvalidParentTagError);
                }
            }
        }


        NbtTag ReadValueAsTag() {
            if (!atValue) {
                // Should never happen
                throw new InvalidOperationException(NoValueToReadError);
            }

            atValue = false;
            switch (TagType) {
                case NbtTagType.Byte:
                    return new NbtByte(TagName, reader.ReadByte());

                case NbtTagType.Short:
                    return new NbtShort(TagName, reader.ReadInt16());

                case NbtTagType.Int:
                    return new NbtInt(TagName, reader.ReadInt32());

                case NbtTagType.Long:
                    return new NbtLong(TagName, reader.ReadInt64());

                case NbtTagType.Float:
                    return new NbtFloat(TagName, reader.ReadSingle());

                case NbtTagType.Double:
                    return new NbtDouble(TagName, reader.ReadDouble());

                case NbtTagType.String:
                    return new NbtString(TagName, reader.ReadString());

                case NbtTagType.ByteArray:
                    byte[] value = reader.ReadBytes(TagLength);
                    if (value.Length < TagLength) {
                        throw new EndOfStreamException();
                    }

                    return new NbtByteArray(TagName, value);

                case NbtTagType.IntArray:
                    int[]? ints = new int[TagLength];
                    for (int i = 0; i < TagLength; i++) {
                        ints[i] = reader.ReadInt32();
                    }

                    return new NbtIntArray(TagName, ints);

                default:
                    throw new InvalidOperationException(NonValueTagError);
            }
        }


        /// <summary> Reads the value as an object of the type specified. </summary>
        /// <typeparam name="T"> The type of the value to be returned.
        /// Tag value should be convertible to this type. </typeparam>
        /// <returns> Tag value converted to the requested type. </returns>
        /// <exception cref="EndOfStreamException"> End of stream has been reached (no more tags can be read). </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="InvalidOperationException"> Value has already been read, or there is no value to read. </exception>
        /// <exception cref="InvalidReaderStateException"> If NbtReader cannot recover from a previous parsing error. </exception>
        /// <exception cref="InvalidCastException"> Tag value cannot be converted to the requested type. </exception>
        public T ReadValueAs<T>() {
            return (T)ReadValue();
        }


        /// <summary> Reads the value as an object of the correct type, boxed.
        /// Cannot be called for tags that do not have a single-object value (compound, list, and end tags). </summary>
        /// <returns> Tag value converted to the requested type. </returns>
        /// <exception cref="EndOfStreamException"> End of stream has been reached (no more tags can be read). </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        /// <exception cref="InvalidOperationException"> Value has already been read, or there is no value to read. </exception>
        /// <exception cref="InvalidReaderStateException"> If NbtReader cannot recover from a previous parsing error. </exception>
        public object ReadValue() {
            if (state == NbtParseState.AtStreamEnd) {
                throw new EndOfStreamException();
            }

            if (!atValue) {
                if (cacheTagValues) {
                    if (valueCache == null) {
                        throw new InvalidOperationException("No value to read.");
                    } else {
                        return valueCache;
                    }
                } else {
                    throw new InvalidOperationException(NoValueToReadError);
                }
            }

            valueCache = null;
            atValue = false;
            object value;
            switch (TagType) {
                case NbtTagType.Byte:
                    value = reader.ReadByte();
                    break;

                case NbtTagType.Short:
                    value = reader.ReadInt16();
                    break;

                case NbtTagType.Float:
                    value = reader.ReadSingle();
                    break;

                case NbtTagType.Int:
                    value = reader.ReadInt32();
                    break;

                case NbtTagType.Double:
                    value = reader.ReadDouble();
                    break;

                case NbtTagType.Long:
                    value = reader.ReadInt64();
                    break;

                case NbtTagType.ByteArray:
                    byte[] valueArr = reader.ReadBytes(TagLength);
                    if (valueArr.Length < TagLength) {
                        throw new EndOfStreamException();
                    }

                    value = valueArr;
                    break;

                case NbtTagType.IntArray:
                    int[]? intValue = new int[TagLength];
                    for (int i = 0; i < TagLength; i++) {
                        intValue[i] = reader.ReadInt32();
                    }

                    value = intValue;
                    break;

                case NbtTagType.String:
                    value = reader.ReadString();
                    break;

                default:
                    throw new InvalidOperationException(NonValueTagError);
            }

            valueCache = cacheTagValues ? value : null;
            return value;
        }


        /// <summary> If the current tag is a List, reads all elements of this list as an array.
        /// If any tags/values have already been read from this list, only reads the remaining unread tags/values.
        /// ListType must be a value type (byte, short, int, long, float, double, or string).
        /// Stops reading after the last list element. </summary>
        /// <typeparam name="T"> Element type of the array to be returned.
        /// Tag contents should be convertible to this type. </typeparam>
        /// <returns> List contents converted to an array of the requested type. </returns>
        /// <exception cref="EndOfStreamException"> End of stream has been reached (no more tags can be read). </exception>
        /// <exception cref="InvalidOperationException"> Current tag is not of type List. </exception>
        /// <exception cref="InvalidReaderStateException"> If NbtReader cannot recover from a previous parsing error. </exception>
        /// <exception cref="NbtFormatException"> If an error occurred while parsing data in NBT format. </exception>
        public T[] ReadListAsArray<T>() {
            switch (state) {
                case NbtParseState.AtStreamEnd:
                    throw new EndOfStreamException();
                case NbtParseState.Error:
                    throw new InvalidReaderStateException(ErroneousStateError);
                case NbtParseState.AtListBeginning:
                    GoDown();
                    ListIndex = 0;
                    TagType = ListType;
                    state = NbtParseState.InList;
                    break;
                case NbtParseState.InList:
                    break;
                default:
                    throw new InvalidOperationException("ReadListAsArray may only be used on List tags.");
            }

            int elementsToRead = ParentTagLength - ListIndex;

            // special handling for reading byte arrays (as byte arrays)
            if (ListType == NbtTagType.Byte && typeof(T) == typeof(byte)) {
                TagsRead += elementsToRead;
                ListIndex = ParentTagLength - 1;
                T[] val = (T[])(object)reader.ReadBytes(elementsToRead);
                if (val.Length < elementsToRead) {
                    throw new EndOfStreamException();
                }

                return val;
            }

            // for everything else, gotta read elements one-by-one
            T[]? result = new T[elementsToRead];
            switch (ListType) {
                case NbtTagType.Byte:
                    for (int i = 0; i < elementsToRead; i++) {
                        result[i] = (T)Convert.ChangeType(reader.ReadByte(), typeof(T));
                    }

                    break;

                case NbtTagType.Short:
                    for (int i = 0; i < elementsToRead; i++) {
                        result[i] = (T)Convert.ChangeType(reader.ReadInt16(), typeof(T));
                    }

                    break;

                case NbtTagType.Int:
                    for (int i = 0; i < elementsToRead; i++) {
                        result[i] = (T)Convert.ChangeType(reader.ReadInt32(), typeof(T));
                    }

                    break;

                case NbtTagType.Long:
                    for (int i = 0; i < elementsToRead; i++) {
                        result[i] = (T)Convert.ChangeType(reader.ReadInt64(), typeof(T));
                    }

                    break;

                case NbtTagType.Float:
                    for (int i = 0; i < elementsToRead; i++) {
                        result[i] = (T)Convert.ChangeType(reader.ReadSingle(), typeof(T));
                    }

                    break;

                case NbtTagType.Double:
                    for (int i = 0; i < elementsToRead; i++) {
                        result[i] = (T)Convert.ChangeType(reader.ReadDouble(), typeof(T));
                    }

                    break;

                case NbtTagType.String:
                    for (int i = 0; i < elementsToRead; i++) {
                        result[i] = (T)Convert.ChangeType(reader.ReadString(), typeof(T));
                    }

                    break;

                default:
                    throw new InvalidOperationException("ReadListAsArray may only be used on lists of value types.");
            }

            TagsRead += elementsToRead;
            ListIndex = ParentTagLength - 1;
            return result;
        }


        /// <summary> Parsing option: Whether NbtReader should skip End tags in ReadToFollowing() automatically while parsing.
        /// Default is <c>true</c>. </summary>
        public bool SkipEndTags { get; set; }

        /// <summary> Parsing option: Whether NbtReader should save a copy of the most recently read tag's value.
        /// Unless CacheTagValues is <c>true</c>, tag values can only be read once. Default is <c>false</c>. </summary>
        public bool CacheTagValues
        {
            get { return cacheTagValues; }
            set
            {
                cacheTagValues = value;
                if (!cacheTagValues) {
                    valueCache = null;
                }
            }
        }

        bool cacheTagValues;


        /// <summary> Returns a String that represents the tag currently being read by this NbtReader instance.
        /// Prints current tag's depth, ordinal number, type, name, and size (for arrays and lists). Does not print value.
        /// Indents the tag according default indentation (NbtTag.DefaultIndentString). </summary>
        public override string ToString() {
            return ToString(false, NbtTag.DefaultIndentString);
        }


        /// <summary> Returns a String that represents the tag currently being read by this NbtReader instance.
        /// Prints current tag's depth, ordinal number, type, name, size (for arrays and lists), and optionally value.
        /// Indents the tag according default indentation (NbtTag.DefaultIndentString). </summary>
        /// <param name="includeValue"> If set to <c>true</c>, also reads and prints the current tag's value. 
        /// Note that unless CacheTagValues is set to <c>true</c>, you can only read every tag's value ONCE. </param>
        public string ToString(bool includeValue) {
            return ToString(includeValue, NbtTag.DefaultIndentString);
        }


        /// <summary> Returns a String that represents the current NbtReader object.
        /// Prints current tag's depth, ordinal number, type, name, size (for arrays and lists), and optionally value. </summary>
        /// <param name="indentString"> String to be used for indentation. May be empty string, but may not be <c>null</c>. </param>
        /// <param name="includeValue"> If set to <c>true</c>, also reads and prints the current tag's value. </param>
        public string ToString(bool includeValue, string indentString) {
            if (indentString == null) throw new ArgumentNullException(nameof(indentString));
            StringBuilder? sb = new StringBuilder();
            for (int i = 0; i < Depth; i++) {
                sb.Append(indentString);
            }

            sb.Append('#').Append(TagsRead).Append(". ").Append(TagType);
            if (IsList) {
                sb.Append('<').Append(ListType).Append('>');
            }

            if (HasLength) {
                sb.Append('[').Append(TagLength).Append(']');
            }

            sb.Append(' ').Append(TagName);
            if (includeValue && (atValue || HasValue && cacheTagValues) && TagType != NbtTagType.IntArray &&
                TagType != NbtTagType.ByteArray) {
                sb.Append(" = ").Append(ReadValue());
            }

            return sb.ToString();
        }


        const string NoValueToReadError = "Value aready read, or no value to read.",
            NonValueTagError = "Trying to read value of a non-value tag.",
            InvalidParentTagError = "Parent tag is neither a Compound nor a List.",
            ErroneousStateError = "NbtReader is in an erroneous state!";
    }


    // Represents state of a node in the NBT file tree, used by NbtReader
    internal sealed class NbtReaderNode {
        public string? ParentName;
        public NbtTagType ParentTagType;
        public NbtTagType ListType;
        public int ParentTagLength;
        public int ListIndex;
    }


    /// <summary> Enumeration of named binary tag types, and their corresponding codes. </summary>
    public enum NbtTagType : byte {
        /// <summary> Placeholder TagType used to indicate unknown/undefined tag type in NbtList. </summary>
        Unknown = 0xff,

        /// <summary> TAG_End: This unnamed tag serves no purpose but to signify the end of an open TAG_Compound. </summary>
        End = 0x00,

        /// <summary> TAG_Byte: A single byte. </summary>
        Byte = 0x01,

        /// <summary> TAG_Short: A single signed 16-bit integer. </summary>
        Short = 0x02,

        /// <summary> TAG_Int: A single signed 32-bit integer. </summary>
        Int = 0x03,

        /// <summary> TAG_Long: A single signed 64-bit integer. </summary>
        Long = 0x04,

        /// <summary> TAG_Float: A single IEEE-754 single-precision floating point number. </summary>
        Float = 0x05,

        /// <summary> TAG_Double: A single IEEE-754 double-precision floating point number. </summary>
        Double = 0x06,

        /// <summary> TAG_Byte_Array: A length-prefixed array of bytes. </summary>
        ByteArray = 0x07,

        /// <summary> TAG_String: A length-prefixed UTF-8 string. </summary>
        String = 0x08,

        /// <summary> TAG_List: A list of nameless tags, all of the same type. </summary>
        List = 0x09,

        /// <summary> TAG_Compound: A set of named tags. </summary>
        Compound = 0x0a,

        /// <summary> TAG_Byte_Array: A length-prefixed array of signed 32-bit integers. </summary>
        IntArray = 0x0b
    }


    /// <summary> An efficient writer for writing NBT data directly to streams.
    /// Each instance of NbtWriter writes one complete file. 
    /// NbtWriter enforces all constraints of the NBT file format
    /// EXCEPT checking for duplicate tag names within a compound. </summary>
    public sealed class NbtWriter {
        const int MaxStreamCopyBufferSize = 8*1024;

        readonly NbtBinaryWriter writer;
        NbtTagType listType;
        NbtTagType parentType;
        int listIndex;
        int listSize;
        Stack<NbtWriterNode>? nodes;


        /// <summary> Initializes a new instance of the NbtWriter class. </summary>
        /// <param name="stream"> Stream to write to. </param>
        /// <param name="rootTagName"> Name to give to the root tag (written immediately). </param>
        /// <remarks> Assumes that data in the stream should be Big-Endian encoded. </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> or <paramref name="rootTagName"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> <paramref name="stream"/> is not writable. </exception>
        public NbtWriter(Stream stream, string rootTagName)
            : this(stream, rootTagName, true) { }


        /// <summary> Initializes a new instance of the NbtWriter class. </summary>
        /// <param name="stream"> Stream to write to. </param>
        /// <param name="rootTagName"> Name to give to the root tag (written immediately). </param>
        /// <param name="bigEndian"> Whether NBT data should be in Big-Endian encoding. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> or <paramref name="rootTagName"/> is <c>null</c>. </exception>
        /// <exception cref="ArgumentException"> <paramref name="stream"/> is not writable. </exception>
        public NbtWriter(Stream stream, string rootTagName, bool bigEndian) {
            if (rootTagName == null) throw new ArgumentNullException(nameof(rootTagName));
            writer = new NbtBinaryWriter(stream, bigEndian);
            writer.Write((byte)NbtTagType.Compound);
            writer.Write(rootTagName);
            parentType = NbtTagType.Compound;
        }


        /// <summary> Gets whether the root tag has been closed.
        /// No more tags may be written after the root tag has been closed. </summary>
        public bool IsDone { get; private set; }

        /// <summary> Gets the underlying stream of the NbtWriter. </summary>
        public Stream BaseStream
        {
            get { return writer.BaseStream; }
        }


        #region Compounds and Lists

        /// <summary> Begins an unnamed compound tag. </summary>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named compound tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        public void BeginCompound() {
            EnforceConstraints(null, NbtTagType.Compound);
            GoDown(NbtTagType.Compound);
        }


        /// <summary> Begins a named compound tag. </summary>
        /// <param name="tagName"> Name to give to this compound tag. May not be null. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed compound tag was expected -OR- a tag of a different type was expected. </exception>
        public void BeginCompound(string tagName) {
            EnforceConstraints(tagName, NbtTagType.Compound);
            GoDown(NbtTagType.Compound);

            writer.Write((byte)NbtTagType.Compound);
            writer.Write(tagName);
        }


        /// <summary> Ends a compound tag. </summary>
        /// <exception cref="NbtFormatException"> Not currently in a compound. </exception>
        public void EndCompound() {
            if (IsDone || parentType != NbtTagType.Compound) {
                throw new NbtFormatException("Not currently in a compound.");
            }

            GoUp();
            writer.Write(NbtTagType.End);
        }


        /// <summary> Begins an unnamed list tag. </summary>
        /// <param name="elementType"> Type of elements of this list. </param>
        /// <param name="size"> Number of elements in this list. Must not be negative. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named list tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="size"/> is negative -OR-
        /// <paramref name="elementType"/> is not a valid NbtTagType. </exception>
        public void BeginList(NbtTagType elementType, int size) {
            if (size < 0) {
                throw new ArgumentOutOfRangeException(nameof(size), "List size may not be negative.");
            }

            if (elementType < NbtTagType.Byte || elementType > NbtTagType.IntArray) {
                throw new ArgumentOutOfRangeException(nameof(elementType));
            }

            EnforceConstraints(null, NbtTagType.List);
            GoDown(NbtTagType.List);
            listType = elementType;
            listSize = size;

            writer.Write((byte)elementType);
            writer.Write(size);
        }


        /// <summary> Begins an unnamed list tag. </summary>
        /// <param name="tagName"> Name to give to this compound tag. May not be null. </param>
        /// <param name="elementType"> Type of elements of this list. </param>
        /// <param name="size"> Number of elements in this list. Must not be negative. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed list tag was expected -OR- a tag of a different type was expected. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="size"/> is negative -OR-
        /// <paramref name="elementType"/> is not a valid NbtTagType. </exception>
        public void BeginList(string tagName, NbtTagType elementType, int size) {
            if (size < 0) {
                throw new ArgumentOutOfRangeException(nameof(size), "List size may not be negative.");
            }

            if (elementType < NbtTagType.Byte || elementType > NbtTagType.IntArray) {
                throw new ArgumentOutOfRangeException(nameof(elementType));
            }

            EnforceConstraints(tagName, NbtTagType.List);
            GoDown(NbtTagType.List);
            listType = elementType;
            listSize = size;

            writer.Write((byte)NbtTagType.List);
            writer.Write(tagName);
            writer.Write((byte)elementType);
            writer.Write(size);
        }


        /// <summary> Ends a list tag. </summary>
        /// <exception cref="NbtFormatException"> Not currently in a list -OR-
        /// not all list elements have been written yet. </exception>
        public void EndList() {
            if (parentType != NbtTagType.List || IsDone) {
                throw new NbtFormatException("Not currently in a list.");
            } else if (listIndex < listSize) {
                throw new NbtFormatException("Cannot end list: not all list elements have been written yet. " +
                                             "Expected: " + listSize + ", written: " + listIndex);
            }

            GoUp();
        }

        #endregion


        #region Value Tags

        /// <summary> Writes an unnamed byte tag. </summary>
        /// <param name="value"> The unsigned byte to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named byte tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        public void WriteByte(byte value) {
            EnforceConstraints(null, NbtTagType.Byte);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed byte tag. </summary>
        /// <param name="tagName"> Name to give to this compound tag. May not be null. </param>
        /// <param name="value"> The unsigned byte to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed byte tag was expected -OR- a tag of a different type was expected. </exception>
        public void WriteByte(string tagName, byte value) {
            EnforceConstraints(tagName, NbtTagType.Byte);
            writer.Write((byte)NbtTagType.Byte);
            writer.Write(tagName);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed double tag. </summary>
        /// <param name="value"> The eight-byte floating-point value to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named double tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        public void WriteDouble(double value) {
            EnforceConstraints(null, NbtTagType.Double);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed byte tag. </summary>
        /// <param name="tagName"> Name to give to this compound tag. May not be null. </param>
        /// <param name="value"> The unsigned byte to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed byte tag was expected -OR- a tag of a different type was expected. </exception>
        public void WriteDouble(string tagName, double value) {
            EnforceConstraints(tagName, NbtTagType.Double);
            writer.Write((byte)NbtTagType.Double);
            writer.Write(tagName);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed float tag. </summary>
        /// <param name="value"> The four-byte floating-point value to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named float tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        public void WriteFloat(float value) {
            EnforceConstraints(null, NbtTagType.Float);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed float tag. </summary>
        /// <param name="tagName"> Name to give to this compound tag. May not be null. </param>
        /// <param name="value"> The four-byte floating-point value to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed float tag was expected -OR- a tag of a different type was expected. </exception>
        public void WriteFloat(string tagName, float value) {
            EnforceConstraints(tagName, NbtTagType.Float);
            writer.Write((byte)NbtTagType.Float);
            writer.Write(tagName);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed int tag. </summary>
        /// <param name="value"> The four-byte signed integer to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named int tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        public void WriteInt(int value) {
            EnforceConstraints(null, NbtTagType.Int);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed int tag. </summary>
        /// <param name="tagName"> Name to give to this compound tag. May not be null. </param>
        /// <param name="value"> The four-byte signed integer to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed int tag was expected -OR- a tag of a different type was expected. </exception>
        public void WriteInt(string tagName, int value) {
            EnforceConstraints(tagName, NbtTagType.Int);
            writer.Write((byte)NbtTagType.Int);
            writer.Write(tagName);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed long tag. </summary>
        /// <param name="value"> The eight-byte signed integer to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named long tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        public void WriteLong(long value) {
            EnforceConstraints(null, NbtTagType.Long);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed long tag. </summary>
        /// <param name="tagName"> Name to give to this compound tag. May not be null. </param>
        /// <param name="value"> The eight-byte signed integer to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed long tag was expected -OR- a tag of a different type was expected. </exception>
        public void WriteLong(string tagName, long value) {
            EnforceConstraints(tagName, NbtTagType.Long);
            writer.Write((byte)NbtTagType.Long);
            writer.Write(tagName);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed short tag. </summary>
        /// <param name="value"> The two-byte signed integer to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named short tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        public void WriteShort(short value) {
            EnforceConstraints(null, NbtTagType.Short);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed short tag. </summary>
        /// <param name="tagName"> Name to give to this compound tag. May not be null. </param>
        /// <param name="value"> The two-byte signed integer to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed short tag was expected -OR- a tag of a different type was expected. </exception>
        public void WriteShort(string tagName, short value) {
            EnforceConstraints(tagName, NbtTagType.Short);
            writer.Write((byte)NbtTagType.Short);
            writer.Write(tagName);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed string tag. </summary>
        /// <param name="value"> The string to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named string tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        public void WriteString(string value) {
            if (value == null) throw new ArgumentNullException(nameof(value));
            EnforceConstraints(null, NbtTagType.String);
            writer.Write(value);
        }


        /// <summary> Writes an unnamed string tag. </summary>
        /// <param name="tagName"> Name to give to this compound tag. May not be null. </param>
        /// <param name="value"> The string to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed string tag was expected -OR- a tag of a different type was expected. </exception>
        public void WriteString(string tagName, string value) {
            if (value == null) throw new ArgumentNullException(nameof(value));
            EnforceConstraints(tagName, NbtTagType.String);
            writer.Write((byte)NbtTagType.String);
            writer.Write(tagName);
            writer.Write(value);
        }

        #endregion


        #region ByteArray and IntArray

        /// <summary> Writes an unnamed byte array tag, copying data from an array. </summary>
        /// <param name="data"> A byte array containing the data to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named byte array tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null </exception>
        public void WriteByteArray(byte[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            WriteByteArray(data, 0, data.Length);
        }


        /// <summary> Writes an unnamed byte array tag, copying data from an array. </summary>
        /// <param name="data"> A byte array containing the data to write. </param>
        /// <param name="offset"> The starting point in <paramref name="data"/> at which to begin writing. Must not be negative. </param>
        /// <param name="count"> The number of bytes to write. Must not be negative. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named byte array tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="offset"/> or
        /// <paramref name="count"/> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null </exception>
        /// <exception cref="ArgumentException"> <paramref name="count"/> is greater than
        /// <paramref name="offset"/> subtracted from the array length. </exception>
        public void WriteByteArray(byte[] data, int offset, int count) {
            CheckArray(data, offset, count);
            EnforceConstraints(null, NbtTagType.ByteArray);
            writer.Write(count);
            writer.Write(data, offset, count);
        }


        /// <summary> Writes a named byte array tag, copying data from an array. </summary>
        /// <param name="tagName"> Name to give to this byte array tag. May not be null. </param>
        /// <param name="data"> A byte array containing the data to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed byte array tag was expected -OR- a tag of a different type was expected. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> or
        /// <paramref name="data"/> is null </exception>
        public void WriteByteArray(string tagName, byte[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            WriteByteArray(tagName, data, 0, data.Length);
        }


        /// <summary> Writes a named byte array tag, copying data from an array. </summary>
        /// <param name="tagName"> Name to give to this byte array tag. May not be null. </param>
        /// <param name="data"> A byte array containing the data to write. </param>
        /// <param name="offset"> The starting point in <paramref name="data"/> at which to begin writing. Must not be negative. </param>
        /// <param name="count"> The number of bytes to write. Must not be negative. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed byte array tag was expected -OR- a tag of a different type was expected. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="offset"/> or
        /// <paramref name="count"/> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> or
        /// <paramref name="data"/> is null </exception>
        /// <exception cref="ArgumentException"> <paramref name="count"/> is greater than
        /// <paramref name="offset"/> subtracted from the array length. </exception>
        public void WriteByteArray(string tagName, byte[] data, int offset, int count) {
            CheckArray(data, offset, count);
            EnforceConstraints(tagName, NbtTagType.ByteArray);
            writer.Write((byte)NbtTagType.ByteArray);
            writer.Write(tagName);
            writer.Write(count);
            writer.Write(data, offset, count);
        }


        /// <summary> Writes an unnamed byte array tag, copying data from a stream. </summary>
        /// <remarks> A temporary buffer will be allocated, of size up to 8192 bytes.
        /// To manually specify a buffer, use one of the other WriteByteArray() overloads. </remarks>
        /// <param name="dataSource"> A Stream from which data will be copied. </param>
        /// <param name="count"> The number of bytes to write. Must not be negative. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named byte array tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count"/> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="dataSource"/> is null. </exception>
        /// <exception cref="ArgumentException"> Given stream does not support reading. </exception>
        public void WriteByteArray(Stream dataSource, int count) {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
            if (!dataSource.CanRead) {
                throw new ArgumentException("Given stream does not support reading.", nameof(dataSource));
            } else if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count), "count may not be negative");
            }

            int bufferSize = Math.Min(count, MaxStreamCopyBufferSize);
            byte[]? streamCopyBuffer = new byte[bufferSize];
            WriteByteArray(dataSource, count, streamCopyBuffer);
        }


        /// <summary> Writes an unnamed byte array tag, copying data from a stream. </summary>
        /// <param name="dataSource"> A Stream from which data will be copied. </param>
        /// <param name="count"> The number of bytes to write. Must not be negative. </param>
        /// <param name="buffer"> Buffer to use for copying. Size must be greater than 0. Must not be null. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named byte array tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count"/> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="dataSource"/> is null. </exception>
        /// <exception cref="ArgumentException"> Given stream does not support reading -OR-
        /// <paramref name="buffer"/> size is 0. </exception>
        public void WriteByteArray(Stream dataSource, int count, byte[] buffer) {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (!dataSource.CanRead) {
                throw new ArgumentException("Given stream does not support reading.", nameof(dataSource));
            } else if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count), "count may not be negative");
            } else if (buffer.Length == 0 && count > 0) {
                throw new ArgumentException("buffer size must be greater than 0 when count is greater than 0",
                                            nameof(buffer));
            }

            EnforceConstraints(null, NbtTagType.ByteArray);
            WriteByteArrayFromStreamImpl(dataSource, count, buffer);
        }


        /// <summary> Writes a named byte array tag, copying data from a stream. </summary>
        /// <remarks> A temporary buffer will be allocated, of size up to 8192 bytes.
        /// To manually specify a buffer, use one of the other WriteByteArray() overloads. </remarks>
        /// <param name="tagName"> Name to give to this byte array tag. May not be null. </param>
        /// <param name="dataSource"> A Stream from which data will be copied. </param>
        /// <param name="count"> The number of bytes to write. Must not be negative. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed byte array tag was expected -OR- a tag of a different type was expected. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count"/> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="dataSource"/> is null. </exception>
        /// <exception cref="ArgumentException"> Given stream does not support reading. </exception>
        public void WriteByteArray(string tagName, Stream dataSource, int count) {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
            if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count), "count may not be negative");
            }

            int bufferSize = Math.Min(count, MaxStreamCopyBufferSize);
            byte[]? streamCopyBuffer = new byte[bufferSize];
            WriteByteArray(tagName, dataSource, count, streamCopyBuffer);
        }


        /// <summary> Writes an unnamed byte array tag, copying data from another stream. </summary>
        /// <param name="tagName"> Name to give to this byte array tag. May not be null. </param>
        /// <param name="dataSource"> A Stream from which data will be copied. </param>
        /// <param name="count"> The number of bytes to write. Must not be negative. </param>
        /// <param name="buffer"> Buffer to use for copying. Size must be greater than 0. Must not be null. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed byte array tag was expected -OR- a tag of a different type was expected. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count"/> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="dataSource"/> is null. </exception>
        /// <exception cref="ArgumentException"> Given stream does not support reading -OR-
        /// <paramref name="buffer"/> size is 0. </exception>
        public void WriteByteArray(string tagName, Stream dataSource, int count,
                                   byte[] buffer) {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (!dataSource.CanRead) {
                throw new ArgumentException("Given stream does not support reading.", nameof(dataSource));
            } else if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count), "count may not be negative");
            } else if (buffer.Length == 0 && count > 0) {
                throw new ArgumentException("buffer size must be greater than 0 when count is greater than 0",
                                            nameof(buffer));
            }

            EnforceConstraints(tagName, NbtTagType.ByteArray);
            writer.Write((byte)NbtTagType.ByteArray);
            writer.Write(tagName);
            WriteByteArrayFromStreamImpl(dataSource, count, buffer);
        }


        /// <summary> Writes an unnamed int array tag, copying data from an array. </summary>
        /// <param name="data"> An int array containing the data to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named int array tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null </exception>
        public void WriteIntArray(int[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            WriteIntArray(data, 0, data.Length);
        }


        /// <summary> Writes an unnamed int array tag, copying data from an array. </summary>
        /// <param name="data"> An int array containing the data to write. </param>
        /// <param name="offset"> The starting point in <paramref name="data"/> at which to begin writing. Must not be negative. </param>
        /// <param name="count"> The number of elements to write. Must not be negative. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// a named int array tag was expected -OR- a tag of a different type was expected -OR-
        /// the size of a parent list has been exceeded. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="offset"/> or
        /// <paramref name="count"/> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null </exception>
        /// <exception cref="ArgumentException"> <paramref name="count"/> is greater than
        /// <paramref name="offset"/> subtracted from the array length. </exception>
        public void WriteIntArray(int[] data, int offset, int count) {
            CheckArray(data, offset, count);
            EnforceConstraints(null, NbtTagType.IntArray);
            writer.Write(count);
            for (int i = offset; i < count; i++) {
                writer.Write(data[i]);
            }
        }


        /// <summary> Writes a named int array tag, copying data from an array. </summary>
        /// <param name="tagName"> Name to give to this int array tag. May not be null. </param>
        /// <param name="data"> An int array containing the data to write. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed int array tag was expected -OR- a tag of a different type was expected. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> or
        /// <paramref name="data"/> is null </exception>
        public void WriteIntArray(string tagName, int[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            WriteIntArray(tagName, data, 0, data.Length);
        }


        /// <summary> Writes a named int array tag, copying data from an array. </summary>
        /// <param name="tagName"> Name to give to this int array tag. May not be null. </param>
        /// <param name="data"> An int array containing the data to write. </param>
        /// <param name="offset"> The starting point in <paramref name="data"/> at which to begin writing. Must not be negative. </param>
        /// <param name="count"> The number of elements to write. Must not be negative. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR-
        /// an unnamed int array tag was expected -OR- a tag of a different type was expected. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="offset"/> or
        /// <paramref name="count"/> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="tagName"/> or
        /// <paramref name="data"/> is null </exception>
        /// <exception cref="ArgumentException"> <paramref name="count"/> is greater than
        /// <paramref name="offset"/> subtracted from the array length. </exception>
        public void WriteIntArray(string tagName, int[] data, int offset, int count) {
            CheckArray(data, offset, count);
            EnforceConstraints(tagName, NbtTagType.IntArray);
            writer.Write((byte)NbtTagType.IntArray);
            writer.Write(tagName);
            writer.Write(count);
            for (int i = offset; i < count; i++) {
                writer.Write(data[i]);
            }
        }

        #endregion


        /// <summary> Writes a NbtTag object, and all of its child tags, to stream.
        /// Use this method sparingly with NbtWriter -- constructing NbtTag objects defeats the purpose of this class.
        /// If you already have lots of NbtTag objects, you might as well use NbtFile to write them all at once. </summary>
        /// <param name="tag"> Tag to write. Must not be null. </param>
        /// <exception cref="NbtFormatException"> No more tags can be written -OR- given tag is unacceptable at this time. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="tag"/> is null </exception>
        public void WriteTag(NbtTag tag) {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            EnforceConstraints(tag.Name, tag.TagType);
            if (tag.Name != "") {
                tag.WriteTag(writer);
            } else {
                tag.WriteData(writer);
            }
        }


        /// <summary> Ensures that file has been written in its entirety, with no tags left open.
        /// This method is for verification only, and does not actually write any data. 
        /// Calling this method is optional (but probably a good idea, to catch any usage errors). </summary>
        /// <exception cref="NbtFormatException"> Not all tags have been closed yet. </exception>
        public void Finish() {
            if (!IsDone) {
                throw new NbtFormatException("Cannot finish: not all tags have been closed yet.");
            }
        }


        void GoDown(NbtTagType thisType) {
            if (nodes == null) {
                nodes = new Stack<NbtWriterNode>();
            }

            NbtWriterNode? newNode = new NbtWriterNode {
                ParentType = parentType,
                ListType = listType,
                ListSize = listSize,
                ListIndex = listIndex
            };
            nodes.Push(newNode);

            parentType = thisType;
            listType = NbtTagType.Unknown;
            listSize = 0;
            listIndex = 0;
        }


        void GoUp() {
            if (nodes == null || nodes.Count == 0) {
                IsDone = true;
            } else {
                NbtWriterNode oldNode = nodes.Pop();
                parentType = oldNode.ParentType;
                listType = oldNode.ListType;
                listSize = oldNode.ListSize;
                listIndex = oldNode.ListIndex;
            }
        }


        void EnforceConstraints(String? name, NbtTagType desiredType) {
            if (IsDone) {
                throw new NbtFormatException("Cannot write any more tags: root tag has been closed.");
            }

            if (parentType == NbtTagType.List) {
                if (name != null) {
                    throw new NbtFormatException("Expecting an unnamed tag.");
                } else if (listType != desiredType) {
                    throw new NbtFormatException("Unexpected tag type (expected: " + listType + ", given: " +
                                                 desiredType);
                } else if (listIndex >= listSize) {
                    throw new NbtFormatException("Given list size exceeded.");
                }

                listIndex++;
            } else if (name == null) {
                throw new NbtFormatException("Expecting a named tag.");
            }
        }


        static void CheckArray(Array data, int offset, int count) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            } else if (offset < 0) {
                throw new ArgumentOutOfRangeException(nameof(offset), "offset may not be negative.");
            } else if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count), "count may not be negative.");
            } else if ((data.Length - offset) < count) {
                throw new ArgumentException("count may not be greater than offset subtracted from the array length.");
            }
        }


        void WriteByteArrayFromStreamImpl(Stream dataSource, int count, byte[] buffer) {
            writer.Write(count);
            int maxBytesToWrite = Math.Min(buffer.Length, NbtBinaryWriter.MaxWriteChunk);
            int bytesWritten = 0;
            while (bytesWritten < count) {
                int bytesToRead = Math.Min(count - bytesWritten, maxBytesToWrite);
                int bytesRead = dataSource.Read(buffer, 0, bytesToRead);
                writer.Write(buffer, 0, bytesRead);
                bytesWritten += bytesRead;
            }
        }
    }


    // Represents state of a node in the NBT file tree, used by NbtWriter
    internal sealed class NbtWriterNode {
        public NbtTagType ParentType;
        public NbtTagType ListType;
        public int ListSize;
        public int ListIndex;
    }


    /// <summary> Delegate used to skip loading certain tags of an NBT stream/file. 
    /// The callback should return "true" for any tag that should be read,and "false" for any tag that should be skipped. </summary>
    /// <param name="tag"> Tag that is being read. Tag's type and name are available,
    /// but the value has not yet been read at this time. Guaranteed to never be <c>null</c>. </param>
    public delegate bool TagSelector(NbtTag tag);


    /// <summary> DeflateStream wrapper that calculates Adler32 checksum of the written data,
    /// to allow writing ZLib header (RFC-1950). </summary>
    internal sealed class ZLibStream : DeflateStream {
        int adler32A = 1,
            adler32B;

        const int ChecksumModulus = 65521;

        public int Checksum
        {
            get { return unchecked((adler32B*65536) + adler32A); }
        }


        void UpdateChecksum(IList<byte> data, int offset, int length) {
            for (int counter = 0; counter < length; ++counter) {
                adler32A = (adler32A + (data[offset + counter]))%ChecksumModulus;
                adler32B = (adler32B + adler32A)%ChecksumModulus;
            }
        }


        public ZLibStream(Stream stream, CompressionMode mode, bool leaveOpen)
            : base(stream, mode, leaveOpen) { }


        public override void Write(byte[] array, int offset, int count) {
            UpdateChecksum(array, offset, count);
            base.Write(array, offset, count);
        }
    }
}
