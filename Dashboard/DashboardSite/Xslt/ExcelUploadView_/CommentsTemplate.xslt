<?xml version="1.0" encoding="utf-8"?>
  <xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
              xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
              xmlns:Utils="xsltUtils:XsltUtils">  
  
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:template  match ="/">
    <xsl:apply-templates> </xsl:apply-templates>

  </xsl:template >

  <xsl:template  match ="CommentModel">
    <xsl:if test="parent::root">
      <div style="font-weight:bold;margin-bottom:4px;">
        <xsl:value-of select="Subject" />
      </div>
    </xsl:if>
    <div>
      <span>
        <xsl:value-of select ="@Username"/> said at <xsl:value-of select ="Utils:GetPreferredTimeFromUtc(@CommentUtcTime)"/>:
      </span>
      <xsl:variable name="NodeId" select="@NodeId"/>
      <input type="hidden" id="nodeId">
        <xsl:attribute name="value">
          <xsl:value-of select="@NodeId" />
        </xsl:attribute>
      </input>
      <span style="float:right;">
        <input type="button" id="btnReply" class="btn" value="Reply" onclick="replyClicked('{$NodeId}')"></input>
      </span>
    </div>
    <div>
      <xsl:apply-templates select="Comment" />
    </div>
    <hr></hr>

    <xsl:if test="ChildComments!=''">
      <div style="padding-left:20px;">
        <xsl:apply-templates select ="ChildComments"></xsl:apply-templates>
        </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match ="ChildComments">
        <xsl:apply-templates select ="CommentModel" />
  </xsl:template>
  <xsl:template match ="Comment">
    <xsl:value-of select ="."/>
  </xsl:template>    
</xsl:stylesheet>
