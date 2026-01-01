namespace YamatoDaiwa.PugCSharpStaticPreview;


public abstract class AssetsProviderForStaticPreview(
  string staticPreviewAbsolutePath,
  string businessRulesOutputFileNameWithExtension,
  string dataOutputFileNameWithExtension,
  string stringResourcesOutputFileNameWithExtension
)
{
  
  /* ━━━ Fields ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ */
  protected readonly string staticPreviewAbsolutePath = staticPreviewAbsolutePath; 
  protected readonly string businessRulesOutputFileNameWithExtension = businessRulesOutputFileNameWithExtension; 
  protected readonly string dataOutputFileNameWithExtension = dataOutputFileNameWithExtension; 
  protected readonly string stringResourcesOutputFileNameWithExtension = stringResourcesOutputFileNameWithExtension;


  /* ━━━ Protected Methods ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ */
  /* ┅┅┅ Abstract ┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅ */
  protected abstract System.Text.Json.Nodes.JsonObject GenerateBusinessRules();
  protected abstract object GenerateData();
  protected abstract System.Text.Json.Nodes.JsonObject GenerateStringResources();
  
  
  /* ┅┅┅ Non-Abstract ┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅ */
  protected void GenerateAccess()
  {
    Parallel.Invoke(
      this.GenerateBusinessRulesAndWriteToFile,
      this.GenerateDataAndWriteToFile,
      this.GenerateStringResourcesAndWriteToFile
    );
  }

  
  /* ┅┅┅ Private Members ┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅┅ */
  private void GenerateBusinessRulesAndWriteToFile()
  {
   
    StreamWriter streamWriter = new(
      path: Path.Combine(this.staticPreviewAbsolutePath, this.businessRulesOutputFileNameWithExtension), 
      append: false
    );
    
    streamWriter.Write(
      this.GenerateBusinessRules().ToJsonString(
        new System.Text.Json.JsonSerializerOptions
        {
          Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
          TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver(),
          WriteIndented = true
        }
      )
    );
    
    streamWriter.Close();
    
  }
  
  private void GenerateDataAndWriteToFile()
  {
    
    StreamWriter streamWriter = new(
      path: Path.Combine(this.staticPreviewAbsolutePath, this.dataOutputFileNameWithExtension), 
      append: false
    );
    
    streamWriter.Write(
      System.Text.Json.JsonSerializer.Serialize(
        this.GenerateData(),
        new System.Text.Json.JsonSerializerOptions
        {
          Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
          TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver(),
          WriteIndented = true
        }
      )
    );
    
    streamWriter.Close();
    
  }
  
  private void GenerateStringResourcesAndWriteToFile()
  {
    
    StreamWriter streamWriter = new(
      path: Path.Combine(
        this.staticPreviewAbsolutePath, 
        this.stringResourcesOutputFileNameWithExtension
      ), 
      append: false
    );
    
    streamWriter.Write(
      this.GenerateStringResources().ToJsonString(
        new System.Text.Json.JsonSerializerOptions
        {
          Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
          TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver(),
          WriteIndented = true
        }
      )
    );
    
    streamWriter.Close();
    
  }
  
}