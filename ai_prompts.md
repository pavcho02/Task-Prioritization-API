# TaskPrioritizatorAPI - Development Prompts Log

This document logs all the prompts used during the development of the **TaskPrioritizatorAPI**, a RESTful API built with **ASP.NET Core Web API** and **SQL Server**, following the MVC structure with a separate **Data Layer** and **Business Layer**.

## **1. Project Setup**

### **Creating the API Project**

```md
I want to build a RESTful API with ASP.NET Core Web API and SQL Server.
```

### **Setting Up MVC Structure**

```md
I want to build on MVC structure with a separate Data Layer and Business Layer.
```

## **2. Database and Migrations**

### **Initializing Migrations**

```md
Add-Migration Initial
```

### **Migration Error Handling**

```md
The database is created but the error is: "Your target project 'TaskPrioritizatorAPI' doesn't match your migrations assembly 'Data'..."
```

## **3. API Development**

### **Fixing a Concurrency Issue in DbContext**

```md
System.InvalidOperationException: 'A second operation was started on this context instance before a previous operation completed...'
```

### **Matching Controller Actions to Query Strings**

```md
How to match action in controller by is there query string and it params?
```

### **Handling HTTP 405 Error**

```md
I got a status 405 method not allowed.
```

## **4. Unit Testing**

### **Adding NUnit Tests**

```md
Now I need to build NUnit tests.
```

### **Fixing Multiple Entry Points Issue**

```md
After adding Unit Test project to the solution there is an error: CS0017 - program has more than one entry point defined.
```

```md
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.2">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.2">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
    <PackageReference Include="Moq" Version="4.20.72" />
    <PackageReference Include="NUnit" Version="4.3.2" />
    <PackageReference Include="NUnit3TestAdapter" Version="5.0.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\Business\Business.csproj" />
    <ProjectReference Include="..\Data\Data.csproj" />
  </ItemGroup>
</Project>
```
