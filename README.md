```
Byte   = UnsignedNumber(1@bytes)
Int32  = SignedNumber(4@bytes, little endian)
UInt32 = UnsignedNumber(4@bytes, little endian)
Int64  = SignedNumber(8@bytes, little endian)
UInt64 = UnsignedNumber(8@bytes, little endian)

Byte   = byte
Int32  =   signed 4@bytes little endian
UInt32 = unsigned 4@bytes little endian
Int64  =   signed 8@bytes big    endian
UInt64 = unsigned 8@bytes big    endian 

Array<Size, Item>
FixedArray<T>(size)

StringUtf8 = Array<UInt32, Utf8>

Signature = record {
    String name,
    FixedArray<Byte>(32) content
}

Unit = record {}

Magic = 0xDEADBEEF

FixedArray<T>(size)

Pair<T1, T2> = record { T1 a, T2 b }

# <                FormatDescription                       >
# <     Declaration     >
# <  Name  >               <        Definition             >
  Named         <T>      = record { String name, T content }

# <                FormatDescription                    >
# <     Declaration    >
# <  Name  >               <        Definition          >
  Dictionary<Key, Value> = Array<UInt32, Map<Key, Value>>

Maybe<T> = enum<Byte> {
    nothing 0 => Unit,
    just    1 => T
}

Block<T>(size) = enum<Byte> {
    empty 0 => Unit,
    block 1 => record {
        Header header,
        FixedArray<T>(size)
    }
}
```