using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace OrderAPI.AWSServices.Menssaging;

public class SQSHandler
{
    private IAmazonSQS _sqsClient;
    private readonly IConfiguration _configuration;

    public SQSHandler(IConfiguration configuration)
    {
        _configuration = configuration;
        _sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
    }
    
    public async Task SendAsync(string messageBody)
    {
        var sqsUrl = await _sqsClient.GetQueueUrlAsync(_configuration["AWS:SQS:QUEUE_NAME"]);
        SendMessageResponse responseSendMsg = await _sqsClient.SendMessageAsync(queueUrl: sqsUrl.QueueUrl, messageBody: messageBody);
        Console.WriteLine($"Message added to queue\n  {sqsUrl}");
        Console.WriteLine($"HttpStatusCode: {responseSendMsg.HttpStatusCode}");
    }
}