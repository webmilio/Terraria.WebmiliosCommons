using System;
using System.Runtime.CompilerServices;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace WebCom.Serializers;

/// <summary>
/// A class which facilitates easy conversion between <see cref="Guid"/> and <see cref="TagCompound"/>.
/// </summary>
// Taken directly from absoluteAquarian's library for compatibility.
// See https://github.com/absoluteAquarian/SerousCommonLib/blob/84078461883c9d6d9757b335364b1d9b0da51bbf/src/API/IO/GuidSerializer.cs
public sealed class GuidSerializer : TagSerializer<Guid, TagCompound>
{
  /// <inheritdoc/>
  public override Guid Deserialize(TagCompound tag)
  {
    int a = tag.GetInt("a");
    short b = tag.GetShort("b");
    short c = tag.GetShort("c");
    byte d = tag.GetByte("d");
    byte e = tag.GetByte("e");
    byte f = tag.GetByte("f");
    byte g = tag.GetByte("g");
    byte h = tag.GetByte("h");
    byte i = tag.GetByte("i");
    byte j = tag.GetByte("j");
    byte k = tag.GetByte("k");
    return new Guid(a, b, c, d, e, f, g, h, i, j, k);
  }

  /// <inheritdoc/>
  public override TagCompound Serialize(Guid value)
  {
    return new()
    {
      ["a"] = Guid__a(ref value),
      ["b"] = Guid__b(ref value),
      ["c"] = Guid__c(ref value),
      ["d"] = Guid__d(ref value),
      ["e"] = Guid__e(ref value),
      ["f"] = Guid__f(ref value),
      ["g"] = Guid__g(ref value),
      ["h"] = Guid__h(ref value),
      ["i"] = Guid__i(ref value),
      ["j"] = Guid__j(ref value),
      ["k"] = Guid__k(ref value)
    };
  }

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_a")]
  extern static ref int Guid__a(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_b")]
  extern static ref short Guid__b(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_c")]
  extern static ref short Guid__c(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_d")]
  extern static ref byte Guid__d(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_e")]
  extern static ref byte Guid__e(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_f")]
  extern static ref byte Guid__f(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_g")]
  extern static ref byte Guid__g(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_h")]
  extern static ref byte Guid__h(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_i")]
  extern static ref byte Guid__i(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_j")]
  extern static ref byte Guid__j(ref Guid guid);

  [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_k")]
  extern static ref byte Guid__k(ref Guid guid);
}