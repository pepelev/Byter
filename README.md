```
!Byter 1.0

Byte           = number byte
ASCII          = number byte allowed [0, 127]
Int32          = number twos complement signed little endian 4@bytes
UInt32         = number               unsigned little endian 4@bytes
Int64          = number twos complement signed big    endian 8@bytes
UInt64         = number               unsigned big    endian 8@bytes
TwoRanges      = number               unsigned little endian 4@bytes allowed [0, 100_000) or [500_000, 1_000_000)
Utf8Char       = number utf8
Zigzag         = number zigzag
LVLQ           = number left  variable length quantity # https://github.com/kstenerud/vlq/blob/master/vlq-specification.md
RVLQ           = number right variable length quantity # https://github.com/kstenerud/vlq/blob/master/vlq-specification.md

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

List<T> = enum<Byte> {
    end     0 => Unit,
    element 1 => record {
        T       head,
        List<T> tail
    }
}

Buffer(size) = record {
    Array<String>(size) names,
    Array<UInt32>(size) values
}

Block<T>(size) = enum<Byte> {
    empty 0 => Unit,
    block 1 => record {
        Header              header,
        FixedArray<T>(size) content
    }
}
```