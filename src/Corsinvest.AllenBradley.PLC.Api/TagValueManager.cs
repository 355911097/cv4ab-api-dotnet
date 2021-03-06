using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Tag local manipulaton value
    /// </summary>
    public class TagValueManager
    {
        private ITag _tag;

        /// <summary>
        /// Byte Length string in header
        /// </summary>
        private const byte BYTE_HEADER_LENGTH_STRING = 4;

        const byte MAX_LENGT_STRING = 82;

        internal TagValueManager(ITag tag) { _tag = tag; }

        private TagValueManager() { }

        /// <summary>
        /// Get local value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal object Get(object obj, int offset = 0)
        {
            var type = obj.GetType();
            if (type.IsArray)
            {
                var array = GetArray(obj);
                for (int i = 0; i < array.Length; i++)
                {
                    var el = array.GetValue(i);
                    array.SetValue(Get(el, offset), i);
                    offset += TagSize.GetSizeFromObject(el);
                }
                return array;
            }
            else
            {
                if (type == typeof(int)) { return GetInt32(offset); }
                else if (type == typeof(uint)) { return GetUInt32(offset); }
                else if (type == typeof(short)) { return GetInt16(offset); }
                else if (type == typeof(ushort)) { return GetUInt16(offset); }
                else if (type == typeof(sbyte)) { return GetInt8(offset); }
                else if (type == typeof(byte)) { return GetUInt8(offset); }
                else if (type == typeof(string)) { return GetString(offset); }
                else if (type == typeof(float) || type == typeof(double)) { return GetFloat32(offset); }
                else if (type.IsClass && !type.IsAbstract) { return GetType(obj, offset); }
                else { throw new Exception("Error data type!"); }
            }
        }

        /// <summary>
        /// Set local value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        internal void Set(object value, int offset = 0)
        {
            var type = value.GetType();
            if (type.IsArray)
            {
                foreach (var el in GetArray(value))
                {
                    Set(el, offset);
                    offset += TagSize.GetSizeFromObject(el);
                }
            }
            else
            {
                if (type == typeof(int)) { SetInt32((int)value, offset); }
                else if (type == typeof(uint)) { SetUInt32((uint)value, offset); }
                else if (type == typeof(short)) { SetInt16((short)value, offset); }
                else if (type == typeof(ushort)) { SetUInt16((ushort)value, offset); }
                else if (type == typeof(sbyte)) { SetInt8((sbyte)value, offset); }
                else if (type == typeof(byte)) { SetUInt8((byte)value, offset); }
                else if (type == typeof(string)) { SetString((string)value, offset); }
                else if (type == typeof(float) || type == typeof(double)) { SetFloat32((float)value, offset); }
                else if (type.IsClass && !type.IsAbstract) { SetType(value, offset); }
                else { throw new Exception("Error data type!"); }
            }
        }

        /// <summary>
        /// Get local value UInt16
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public ushort GetUInt16(int offset = 0) { return NativeMethod.plc_tag_get_uint16(_tag.Handle, offset); }

        /// <summary>
        /// Set local value UInt16
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        public void SetUInt16(ushort value, int offset = 0) { NativeMethod.plc_tag_set_uint16(_tag.Handle, offset, value); }

        /// <summary>
        /// Get local value Int16
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public short GetInt16(int offset = 0) { return NativeMethod.plc_tag_get_int16(_tag.Handle, offset); }

        /// <summary>
        /// Set local value Int16
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        public void SetInt16(short value, int offset = 0) { NativeMethod.plc_tag_set_int16(_tag.Handle, offset, value); }

        /// <summary>
        /// Get local value UInt8
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte GetUInt8(int offset = 0) { return NativeMethod.plc_tag_get_uint8(_tag.Handle, offset); }

        /// <summary>
        /// Set local value UInt8
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        public void SetUInt8(byte value, int offset = 0) { NativeMethod.plc_tag_set_uint8(_tag.Handle, offset, value); }

        /// <summary>
        /// Get local value Int8
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public sbyte GetInt8(int offset = 0) { return NativeMethod.plc_tag_get_int8(_tag.Handle, offset); }

        /// <summary>
        /// Set local value Int8
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        public void SetInt8(sbyte value, int offset = 0) { NativeMethod.plc_tag_set_int8(_tag.Handle, offset, value); }

        /// <summary>
        /// Get local value UInt32
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public uint GetUInt32(int offset = 0) { return NativeMethod.plc_tag_get_uint32(_tag.Handle, offset); }

        /// <summary>
        /// Set local value UInt32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        public void SetUInt32(uint value, int offset = 0) { NativeMethod.plc_tag_set_uint32(_tag.Handle, offset, value); }

        /// <summary>
        /// Get local value Int32
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public int GetInt32(int offset = 0) { return NativeMethod.plc_tag_get_int32(_tag.Handle, offset); }

        /// <summary>
        /// Set local value Int32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        public void SetInt32(int value, int offset = 0) { NativeMethod.plc_tag_set_int32(_tag.Handle, offset, value); }

        /// <summary>
        /// Get local value Float32
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public float GetFloat32(int offset = 0) { return NativeMethod.plc_tag_get_float32(_tag.Handle, offset); }

        /// <summary>
        /// Set local value Float32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        public void SetFloat32(float value, int offset = 0) { NativeMethod.plc_tag_set_float32(_tag.Handle, offset, value); }

        /// <summary>
        /// Get local value String
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public string GetString(int offset = 0)
        {
            var sb = new StringBuilder();

            //max length string
            var length = GetInt32(offset);

            //read only length of string
            for (var i = 0; i < length; i++) { sb.Append((char)GetUInt8(offset + BYTE_HEADER_LENGTH_STRING + i)); }
            return sb.ToString();
        }

        /// <summary>
        /// Set local value String
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        public void SetString(string value, int offset = 0)
        {
            if (value.Length > MAX_LENGT_STRING) { throw new IndexOutOfRangeException($"Length strign <= {MAX_LENGT_STRING}!"); }

            //set length
            SetInt32(value.Length, offset);

            var strIdx = 0;

            //copy data
            for (strIdx = 0; strIdx < value.Length; strIdx++)
            {
                SetUInt8((byte)value[strIdx], offset + BYTE_HEADER_LENGTH_STRING + strIdx);
            }

            // pad with zeros
            for (; strIdx < MAX_LENGT_STRING; strIdx++)
            {
                SetUInt8(0, offset + BYTE_HEADER_LENGTH_STRING + strIdx);
            }
        }

        /// <summary>
        /// Get bit from index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetBit(int index)
        {
            if (_tag.Size * 8 <= index) { throw new IndexOutOfRangeException("Index out of bound!"); }
            if (_tag.Size == TagSize.INT32) { return (GetUInt32(0) & (1 << index)) != 0; }
            else if (_tag.Size == TagSize.INT16) { return (GetUInt16(0) & (1 << index)) != 0; }
            else if (_tag.Size == TagSize.INT8) { return (GetUInt8(0) & (1 << index)) != 0; }

            throw new Exception("Error data type!");
        }

        /// <summary>
        /// Set bit from index and value
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetBit(int index, bool value)
        {
            if (_tag.Size * 8 <= index) { throw new IndexOutOfRangeException("Index out of bound!"); }
            var index2 = Math.Pow(2, index);

            if (_tag.Size == TagSize.INT32)
            {
                var data = GetUInt32(0);
                SetUInt32(value ? data | (uint)index2 : data ^ (uint)index2, 0);
            }
            else if (_tag.Size == TagSize.INT16)
            {
                var data = GetUInt16(0);
                SetUInt16((ushort)(value ? data | (ushort)index2 : data ^ (ushort)index2), 0);
            }
            else if (_tag.Size == TagSize.INT8)
            {
                var data = GetUInt8(0);
                SetUInt8((byte)(value ? data | (byte)index2 : data ^ (byte)index2), 0);
            }
        }

        /// <summary>
        /// Get bit array from value
        /// </summary>
        /// <returns></returns>
        public BitArray GetBits()
        {
            if (_tag.Size == TagSize.INT32) { return new BitArray(new[] { (int)GetUInt32(0) }); }
            else if (_tag.Size == TagSize.INT16) { return new BitArray(new[] { (int)GetUInt16(0) }); }
            else if (_tag.Size == TagSize.INT8) { return new BitArray(new[] { GetUInt8(0) }); }

            throw new Exception("Error data type!");
        }

        /// <summary>
        /// Set bits from BitArray
        /// </summary>
        /// <param name="bits"></param>
        public void SetBits(BitArray bits)
        {
            if (bits == null) { throw new ArgumentNullException("binary"); }
            if (bits.Length > 32) { throw new ArgumentOutOfRangeException("must be at most 32 bits long"); }

            for (int i = 0; i < _tag.Size * 8; i++) { SetBit(i, bits[i]); }
        }

        /// <summary>
        /// Set local valute from type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        public void SetType(object obj, int offset = 0)
        {
            foreach (var property in GetAccessableProperties(obj.GetType()))
            {
                var value = property.GetValue(obj);
                Set(value, offset);
                offset += TagSize.GetSizeFromObject(value);
            }
        }

        /// <summary>
        /// Get local value form type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public object GetType(object obj, int offset = 0)
        {
            foreach (var property in GetAccessableProperties(obj.GetType()))
            {
                var value = property.GetValue(obj);
                property.SetValue(obj, Get(value, offset));
                offset += TagSize.GetSizeFromObject(value);
            }

            return obj;
        }

        internal static IEnumerable<PropertyInfo> GetAccessableProperties(Type type)
        {
            return type.GetProperties(BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public)
                       .Where(p => p.GetSetMethod() != null);
        }

        internal static Array GetArray(object value)
        {
            var array = (Array)value;
            if (array.Length <= 0)
            {
                throw new Exception("Cannot determine size of class, " +
                                    "because an array is defined which has no fixed size greater than zero.");
            }
            return array;
        }

        /// <summary>
        /// Fix string null to empty.
        /// </summary>
        /// <param name="obj"></param>
        internal static void FixStringNullToEmpty(object obj)
        {
            var type = obj.GetType();
            if (type == typeof(string))
            {
                if (obj == null) { obj = string.Empty; }
            }
            else if (type.IsArray && type.GetElementType() == typeof(string))
            {
                var array = GetArray(obj);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array.GetValue(i) == null) { array.SetValue(string.Empty, i); }
                }
            }
            else if (type.IsClass && !type.IsAbstract)
            {
                foreach (var property in GetAccessableProperties(type))
                {
                    if (property.PropertyType == typeof(string))
                    {
                        if (property.GetValue(obj) == null) { property.SetValue(obj, string.Empty); }
                    }
                    else
                    {
                        FixStringNullToEmpty(property.GetValue(obj));
                    }
                }
            }
        }
    }
}