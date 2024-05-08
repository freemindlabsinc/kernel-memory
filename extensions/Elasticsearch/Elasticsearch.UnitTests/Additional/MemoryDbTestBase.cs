﻿// Copyright (c) Microsoft. All rights reserved.

using Elastic.Clients.Elasticsearch;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI;
using Microsoft.KernelMemory.AI.OpenAI;
using Microsoft.KernelMemory.MemoryDb.Elasticsearch;
using Microsoft.KernelMemory.MemoryStorage;
using Microsoft.KM.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Elasticsearch.FunctionalTests.Additional;

/// <summary>
/// A simple base class for Elasticsearch tests.
/// It ensures that all indices created by the test methods of the derived class are
/// deleted before and after the tests. This ensures that Elasticsearch is left in a clean state
/// or that subsequent tests don't fail because of left-over indices.
/// </summary>
public abstract class MemoryDbTestBase : BaseFunctionalTestCase//, IAsyncLifetime
{
    protected MemoryDbTestBase(IConfiguration cfg, ITestOutputHelper output)//, ElasticsearchClient client)
        : base(cfg, output)
    {
        this.Output = output ?? throw new ArgumentNullException(nameof(output));


#pragma warning disable KMEXP01 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        this.TextEmbeddingGenerator = new OpenAITextEmbeddingGenerator(
            config: base.OpenAiConfig,
            textTokenizer: default,
            loggerFactory: default);
#pragma warning restore KMEXP01 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        this.Client = new ElasticsearchClient(base.ElasticsearchConfig.ToElasticsearchClientSettings());
        this.MemoryDb = new ElasticsearchMemory(base.ElasticsearchConfig, this.Client, this.TextEmbeddingGenerator, default);


    }

    public ITestOutputHelper Output { get; }
    public ElasticsearchClient Client { get; }
    public IMemoryDb MemoryDb { get; }
    public ITextEmbeddingGenerator TextEmbeddingGenerator { get; }


    //public async Task InitializeAsync()
    //{
    //    // Within a single test class, the tests are executed sequentially by default so
    //    // there is no chance for a method to finish and delete indices of other methods before the next
    //    // method starts executing.
    //    //var delIndexResponse = await this.Client.Indices.DeleteAsync(indices: this.con)
    //    //                                                .ConfigureAwait(false);
    //
    //    var indicesFound = await this.Client.DeleteIndicesOfTestAsync(this.GetType(), this.IndexNameHelper).ConfigureAwait(false);
    //
    //    if (indicesFound.Any())
    //    {
    //        this.Output.WriteLine($"Deleted left-over test indices: {string.Join(", ", indicesFound)}");
    //        this.Output.WriteLine("");
    //    }
    //}
    //
    //public async Task DisposeAsync()
    //{
    //    var indicesFound = await this.Client.DeleteIndicesOfTestAsync(this.GetType(), this.IndexNameHelper).ConfigureAwait(false);
    //
    //    if (indicesFound.Any())
    //    {
    //        this.Output.WriteLine($"Deleted test indices: {string.Join(", ", indicesFound)}");
    //        this.Output.WriteLine("");
    //    }
    //}
}
