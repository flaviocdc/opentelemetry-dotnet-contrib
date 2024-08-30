// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

public sealed class OneCollectorLogExporterOptionsTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("_")]
    [InlineData("123")]
    public void InvalidDefaultEventNamespaceTests(string? defaultEventNamespace)
    {
        var options = new OneCollectorLogExporterOptions()
        {
            ConnectionString = "InstrumentationKey=token-extrainformation",
            DefaultEventNamespace = defaultEventNamespace!,
        };

        Assert.Throws<OneCollectorExporterValidationException>(
            () => options.Validate());
    }

    [Theory]
    [InlineData("")]
    [InlineData("A.B")]
    [InlineData("default")]
    public void ValidDefaultEventNamespaceTests(string? defaultEventNamespace)
    {
        var options = new OneCollectorLogExporterOptions()
        {
            ConnectionString = "InstrumentationKey=token-extrainformation",
            DefaultEventNamespace = defaultEventNamespace!,
            DefaultEventName = "Logs",
        };

        options.Validate();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("_")]
    [InlineData(".")]
    [InlineData("123")]
    public void InvalidDefaultEventNameTests(string? defaultEventName)
    {
        var options = new OneCollectorLogExporterOptions()
        {
            ConnectionString = "InstrumentationKey=token-extrainformation",
            DefaultEventName = defaultEventName!,
        };

        Assert.Throws<OneCollectorExporterValidationException>(
            () => options.Validate());
    }

    [Theory]
    [InlineData("default123")]
    public void ValidDefaultEventNameTests(string? defaultEventName)
    {
        var options = new OneCollectorLogExporterOptions()
        {
            ConnectionString = "InstrumentationKey=token-extrainformation",
            DefaultEventName = defaultEventName!,
        };

        options.Validate();
    }

    [Theory]
    [InlineData("A", "B")]
    [InlineData("", "Log")]
    public void InvalidDefaultEventFullNameTests(string defaultEventNamespace, string defaultEventName)
    {
        var options = new OneCollectorLogExporterOptions()
        {
            ConnectionString = "InstrumentationKey=token-extrainformation",
            DefaultEventNamespace = defaultEventNamespace,
            DefaultEventName = defaultEventName,
        };

        Assert.Throws<OneCollectorExporterValidationException>(
            () => options.Validate());
    }

    [Theory]
    [InlineData("A", "CD")]
    [InlineData("", "ABCD")]
    public void ValidDefaultEventFullNameTests(string defaultEventNamespace, string defaultEventName)
    {
        var options = new OneCollectorLogExporterOptions()
        {
            ConnectionString = "InstrumentationKey=token-extrainformation",
            DefaultEventNamespace = defaultEventNamespace,
            DefaultEventName = defaultEventName,
        };

        options.Validate();
    }

    [Theory]
    [InlineData("", "Value")]
    [InlineData("Prefix", null)]
    [InlineData("Prefix", "")]
    [InlineData("*", "")]
    [InlineData("Prefix", "ABC")]
    [InlineData("Prefix", "_.EventName")]
    [InlineData("Prefix", "Namespace.Event_Name")]
    [InlineData("Prefix", "Namespace.")]
    public void InvalidEventFullNameMappingTests(string key, string? value)
    {
        var options = new OneCollectorLogExporterOptions()
        {
            ConnectionString = "InstrumentationKey=token-extrainformation",
            EventFullNameMappings = new Dictionary<string, string>()
            {
                { key, value! },
            },
        };

        Assert.Throws<OneCollectorExporterValidationException>(
            () => options.Validate());
    }

    [Theory]
    [InlineData("*", "ABCD")]
    [InlineData("*", "A.BC")]
    [InlineData("*", "A.B.C")]
    [InlineData("*", "*")]
    [InlineData("*", "*.EventName")]
    public void ValidEventFullNameMappingTests(string key, string? value)
    {
        var options = new OneCollectorLogExporterOptions()
        {
            ConnectionString = "InstrumentationKey=token-extrainformation",
            EventFullNameMappings = new Dictionary<string, string>()
            {
                { key, value! },
            },
        };

        options.Validate();
    }
}
