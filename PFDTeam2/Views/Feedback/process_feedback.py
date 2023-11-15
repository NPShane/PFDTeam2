import spacy

def process_feedback(feedback_text):
    nlp = spacy.load("en_core_web_sm")
    doc = nlp(feedback_text)

    # Example 1: Print tokens and part-of-speech tags
    for token in doc print(f"{token.text}: {token.pos_}")
    
    # Example 2: Extract named entities
    entities = [ent.text for ent in doc.ents]
    print("Named Entities:", entities)

    # Additional processing logic can be added based on requirements

if __name__ == "__main__":
    # Example: Process feedback from command line
    feedback = input("Enter feedback: ")
    process_feedback(feedback)
