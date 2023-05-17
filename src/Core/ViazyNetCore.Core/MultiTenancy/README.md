

### ICurrentTenant 具有作用域，在应用程序中，可通过Change切换租户对象，在同线程的上下文中具有保护。
```csharp
    [Fact]
    public void Should_Get_Changed_Tenant_If()
    {
        _currentTenant.Id.ShouldBe(null);

        using (_currentTenant.Change(_tenantAId))
        {
            _currentTenant.Id.ShouldBe(_tenantAId);

            using (_currentTenant.Change(_tenantBId))
            {
                _currentTenant.Id.ShouldBe(_tenantBId);
            }

            _currentTenant.Id.ShouldBe(_tenantAId);
        }

        _currentTenant.Id.ShouldBeNull();
    }
```