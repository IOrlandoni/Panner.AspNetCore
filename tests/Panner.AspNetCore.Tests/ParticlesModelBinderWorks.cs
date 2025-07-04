namespace Panner.AspNetCore.Tests
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using NSubstitute;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public abstract class ParticlesModelBinderWorks<TBinder>
        where TBinder : IModelBinder
    {
        abstract protected TBinder Create(IPContext context);

        public ParticlesModelBinderWorks(){}

        private IPContext GetPannerContext()
            =>  Substitute.For<IPContext>();

        private ModelBindingContext GetBindingContext(string modelName)
        {
            var bindingContext = Substitute.For<ModelBindingContext>();
            var valueProvider = Substitute.For<IValueProvider>();
            var modelStateDict = Substitute.For<ModelStateDictionary>();

            bindingContext.ModelName.Returns(modelName);
            bindingContext.ValueProvider.Returns(valueProvider);
            bindingContext.ModelState.Returns(modelStateDict);

            return bindingContext;
        }

        [Fact]
        public void NullIPContextThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var x = Create(null);
            });
        }

        [Fact]
        public async Task NullBindingContextThrows()
        {
            var x = Create(GetPannerContext());

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await x.BindModelAsync(null);
            });
        }

        [Fact]
        public async Task EmptyEnumerableIfNotFoundInBindingContextAsync()
        {
            var modelName = Guid.NewGuid().ToString();
            var bindingContext = GetBindingContext(modelName);

            bindingContext.ValueProvider.GetValue(modelName).Returns(ValueProviderResult.None);

            var x = Create(GetPannerContext());
            await x.BindModelAsync(bindingContext);

            Assert.True(bindingContext.Result.IsModelSet);
            Assert.Empty((IEnumerable)bindingContext.Result.Model);
        }

        [Fact]
        public async Task FailsIfTheInputCantBeParsedByTheContext()
        {
            var modelName = Guid.NewGuid().ToString();
            var valueResult = Guid.NewGuid().ToString();

            var bindingContext = GetBindingContext(modelName);
            bindingContext.ValueProvider.GetValue(modelName).Returns(new ValueProviderResult(valueResult));

            var x = Create(GetPannerContext());
            await x.BindModelAsync(bindingContext);

            Assert.False(bindingContext.Result.IsModelSet);
            Assert.False(bindingContext.ModelState.IsValid);
            Assert.True(bindingContext.ModelState.ContainsKey(modelName));
            Assert.Equal(1, bindingContext.ModelState.ErrorCount);
            Assert.Contains("Could not parse provided", bindingContext.ModelState[modelName].Errors.Single().ErrorMessage);
        }

    }
}
