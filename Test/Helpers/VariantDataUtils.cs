using Autodesk.Navisworks.Api;

namespace Test.Helpers;

public static class VariantDataUtils
{
    public static dynamic ToVarDisplayString(this VariantData variantData)
    {
        switch (variantData.DataType)
        {
            case VariantDataType.None:
                return variantData.ToString();
            case VariantDataType.Double:
                return variantData.ToDouble();
            case VariantDataType.Int32:
                return variantData.ToInt32();
            case VariantDataType.Boolean:
                return variantData.ToBoolean();
            case VariantDataType.DisplayString:
                return variantData.ToDisplayString();
            case VariantDataType.DateTime:
                return variantData.ToDateTime();
            case VariantDataType.DoubleLength:
                return variantData.ToDoubleLength();
            case VariantDataType.DoubleAngle:
                return variantData.ToDoubleAngle();
            case VariantDataType.NamedConstant:
                return variantData.ToNamedConstant();
            case VariantDataType.IdentifierString:
                return variantData.ToIdentifierString();
            case VariantDataType.DoubleArea:
                return variantData.ToDoubleArea();
            case VariantDataType.DoubleVolume:
                return variantData.ToDoubleVolume();
            case VariantDataType.Point3D:
                return variantData.ToPoint3D();
            case VariantDataType.Point2D:
                return variantData.ToPoint2D();
            default:
                return variantData.ToString();
        }
    }
}