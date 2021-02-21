echo "Utility.Ef  clean starting ..."
dotnet clean
echo "Utility.Ef  clean finished ..."

echo "Utility.Ef  build starting ..."
dotnet build
echo "Utility.Ef  build finished ..."

echo "Utility.Ef  pack starting ..."
dotnet pack
echo "Utility.Ef  pack finished ..."

rem 打包成功 发布到 nexus nuget 中

